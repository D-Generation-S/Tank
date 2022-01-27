using DebugFramework.DataTypes;
using DebugFramework.DataTypes.Responses;
using DebugFramework.Streaming.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming
{
    public class UdpServer : IDisposable
    {
        private readonly Dictionary<Type, Func<UdpClient, Type, UdpPackage<BaseDataType>, IPEndPoint, uint, byte[]>> responses;

        private object lockObject = new object();
        private List<IPEndPoint> connectedClients;

        private uint responsePackageNumber;
        private object responsePackageNumberLock = new object();


        private bool serverRunning;
        //private int updatePort;

        public UdpServer()
        {
            connectedClients = new List<IPEndPoint>();
            responsePackageNumber = 0;
            responses = new Dictionary<Type, Func<UdpClient, Type, UdpPackage<BaseDataType>, IPEndPoint, uint, byte[]>>();
            AddResponse(typeof(ConnectRequest), (client, type, data, endpoint, nextPackageNumber) =>
            {
                BaseDataType baseData = data.GetBasePayload();
                if (baseData.GetRealType() == typeof(ConnectRequest))
                {
                    UdpPackage<RecievedConfirmation> package = new UdpPackage<RecievedConfirmation>();
                    RecievedConfirmation payload = new RecievedConfirmation();
                    package.Init(nextPackageNumber, DataIdentifier.Response, payload);
                    return package.GetDataStream();
                }
                return new byte[0];

            });

            AddResponse(typeof(UnknownType), (client, type, data, endpoint, nextPackageNumber) =>
            {
                UdpPackage<UnknownType> package = new UdpPackage<UnknownType>();
                package.Init(nextPackageNumber, DataIdentifier.Response, new UnknownType());
                return package.GetDataStream();
            });
        }

        public void AddResponse(Type dataType, Func<UdpClient, Type, UdpPackage<BaseDataType>, IPEndPoint, uint, byte[]> responseAction)
        {
            if (responses.ContainsKey(dataType))
            {
                responses.Remove(dataType);
            }
            responses.Add(dataType, responseAction);
        }

        public async Task StartServer()
        {
            serverRunning = true;

            await Task.Run(async () =>
            {
                uint packageNumer = 0;
                int communicationPort = await FindFreePort(1024, 49151);
                int updatePort = await FindFreePort(1024, 49151);
                StartResponseServer(communicationPort);
                StartMainUpdateServer(updatePort);
                NetworkInterface fastesInterface = NetworkInterface.GetAllNetworkInterfaces()
                                                                   .ToList()
                                                                   .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                                                                   .Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet || nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                                                                   .OrderBy(nic => nic.Speed)
                                                                   .FirstOrDefault();
                UnicastIPAddressInformation unicastAdress = fastesInterface.GetIPProperties().UnicastAddresses
                                                                           .FirstOrDefault(unicast => unicast.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                IPAddress boradcastAddress = await GetBroacastAdressAsync(unicastAdress.Address, unicastAdress.IPv4Mask);

                Socket broadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint broadcastEndpoint = new IPEndPoint(boradcastAddress, Configuration.BROADCAST_IP);

                UdpPackage<BroadcastData> broadcastMessage = new UdpPackage<BroadcastData>();
                BroadcastData broadcastData = new BroadcastData()
                {
                    ServerName = Environment.MachineName,
                    IpAddress = unicastAdress.Address.ToString(),
                    UpdatePort = updatePort,
                    CommunicationPort = communicationPort
                };

                while (serverRunning)
                {
                    broadcastMessage.Init(packageNumer, DataIdentifier.Broadcast, broadcastData);
                    byte[] dataToSend = broadcastMessage.GetDataStream();
                    broadcastSocket.SendTo(dataToSend, broadcastEndpoint);
                    await Task.Delay(1000);
                    packageNumer++;
                }
            });
        }

        public void StopServer()
        {
            serverRunning = false;
        }

        public async Task<IPAddress> GetBroacastAdressAsync(IPAddress ipAddress, IPAddress subnetMask)
        {
            return await Task.Run(() =>
            {
                byte[] ipAddressBytes = ipAddress.GetAddressBytes();
                byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

                if (ipAddressBytes.Length != subnetMaskBytes.Length)
                {
                    return null;
                }

                byte[] broadcastAddress = new byte[ipAddressBytes.Length];
                for (int i = 0; i < broadcastAddress.Length; i++)
                {
                    broadcastAddress[i] = (byte)(ipAddressBytes[i] | (subnetMaskBytes[i] ^ 255));
                }
                return new IPAddress(broadcastAddress);
            });
        }

        public async Task<int> FindFreePort(int startRange, int endRange)
        {
            startRange = startRange == 0 ? 1 : startRange;
            return await Task.Run(() =>
            {
                int returnInt = 0;
                List<int> portArray = new List<int>();
                IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                portArray.AddRange(iPGlobalProperties.GetActiveTcpConnections()
                                                     .Where(x => x.LocalEndPoint.Port >= startRange)
                                                     .Where(x => x.LocalEndPoint.Port <= endRange)
                                                     .Select(x => x.LocalEndPoint.Port));
                portArray.AddRange(iPGlobalProperties.GetActiveTcpListeners()
                                         .Where(x => x.Port >= startRange)
                                         .Where(x => x.Port <= endRange)
                                         .Select(x => x.Port));
                portArray.AddRange(iPGlobalProperties.GetActiveUdpListeners()
                                         .Where(x => x.Port >= startRange)
                                         .Where(x => x.Port <= endRange)
                                         .Select(x => x.Port));

                portArray.Sort();

                for (int i = startRange; i < endRange; i++)
                {
                    if (!portArray.Contains(i))
                    {
                        returnInt = i;
                    }
                }

                return returnInt;
            });

        }

        private async Task StartResponseServer(int port)
        {
            UdpClient udpClient = new UdpClient(port, AddressFamily.InterNetwork);
            IPEndPoint responseEndpoint = new IPEndPoint(IPAddress.Any, port);
            await Task.Run(() =>
            {
                UdpPackage<BaseDataType> package = new UdpPackage<BaseDataType>();
                while (serverRunning)
                {
                    byte[] recievedData = udpClient.Receive(ref responseEndpoint);
                    package.Init(recievedData);
                    BaseDataType data = package.GetBasePayload();
                    Type type = data.GetRealType();
                    if (responses.ContainsKey(type))
                    {
                        lock (responsePackageNumberLock)
                        {
                            byte[] dataToSend = responses[type](udpClient, type, package, responseEndpoint, responsePackageNumber);
                            if (dataToSend.Length > 0)
                            {
                                udpClient.Send(dataToSend, dataToSend.Length, responseEndpoint);
                                responsePackageNumber++;
                            }

                        }

                    }
                }

            });
        }

        private async Task StartMainUpdateServer(int port)
        {
            UdpClient udpClient = new UdpClient(port, AddressFamily.InterNetwork);

            await Task.Run(() =>
            {

            });
        }

        public void Dispose()
        {
            StopServer();
        }
    }
}

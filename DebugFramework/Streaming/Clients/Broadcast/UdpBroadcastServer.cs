using DebugFramework.DataTypes;
using DebugFramework.Streaming.Clients.Communication;
using DebugFramework.Streaming.Package;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Broadcast
{
    public class UdpBroadcastServer<T> : BroadcastClient, IDisposable where T : BaseDataType
    {
        private bool serverRunning;
        private uint packageNumber;

        private object broadcastDataLock;
        private T broadcastData;

        private object broadcastClientLock;
        private readonly IPEndPoint broadcastEndPoint;
        private readonly UdpSendClient broadcastClient;

        public UdpBroadcastServer(int broadcastPort)
        {
            packageNumber = 0;
            broadcastDataLock = new object();
            broadcastClientLock = new object();

            UnicastIPAddressInformation unicastAdress = GetUnicastAdressInformation();
            IPAddress broadcastAddress = GetBroadcastAddress(unicastAdress.Address, unicastAdress.IPv4Mask);
            broadcastEndPoint = new IPEndPoint(broadcastAddress, broadcastPort);
            broadcastClient = new UdpSendClient(new UdpClient(AddressFamily.InterNetwork) { EnableBroadcast = true });
        }

        public async Task StartBroadcast(UdpPackage packageToBroadcast, T broadcastData)
        {
            serverRunning = true;
            this.broadcastData = broadcastData;
            await Task.Run(async () =>
            {
                while (serverRunning)
                {
                    lock (broadcastDataLock)
                    {
                        packageToBroadcast.Init(packageNumber, DataIdentifier.Broadcast, this.broadcastData);
                    }
                    if (serverRunning)
                    {
                        lock (broadcastClientLock)
                        {
                            broadcastClient.SendTo(broadcastEndPoint, packageToBroadcast);
                        }
                        await Task.Delay(1000);
                        packageNumber++;
                    }
                }
            });
        }

        public void SendBroadcast(UdpPackage packageToBroadcast)
        {
            broadcastClient.SendTo(broadcastEndPoint, packageToBroadcast);
        }

        public bool ChangeBroadcastContent(T newData)
        {
            if (!serverRunning && newData != null)
            {
                return false;
            }
            lock (broadcastDataLock)
            {
                broadcastData = newData;
            }
            return true;
        }

        public void StopBroadcast()
        {
            serverRunning = false;
        }

        public void Dispose()
        {
            StopBroadcast();
            broadcastClient.Dispose();
        }
    }
}

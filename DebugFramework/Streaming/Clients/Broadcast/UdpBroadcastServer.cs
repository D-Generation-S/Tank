using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Broadcast
{
    public class UdpBroadcastServer<T> : BroadcastClient, IDisposable where T : BaseDataType
    {
        private bool serverRunning;
        private uint packageNumber;
        private int broadcastPort;

        private object broadcastDataLock;
        private T broadcastData;

        public UdpBroadcastServer(int broadcastPort)
        {
            packageNumber = 0;
            this.broadcastPort = broadcastPort;
            broadcastDataLock = new object();
        }

        public async Task StartBroadcast(UdpPackage<T> packageToBroadcast, T broadcastData)
        {
            serverRunning = true;
            this.broadcastData = broadcastData;
            await Task.Run(async () =>
            {
                UnicastIPAddressInformation unicastAdress = await GetUnicastAdressInformationAsync();
                IPAddress broadcastAddress = await GetBroacastAddressAsync(unicastAdress.Address, unicastAdress.IPv4Mask);

                UdpBroadcastSender<T> broadcastClient = new UdpBroadcastSender<T>(broadcastPort);
                while (serverRunning)
                {
                    lock (broadcastDataLock)
                    {
                        packageToBroadcast.Init(packageNumber, DataIdentifier.Broadcast, this.broadcastData);
                    }
                    broadcastClient.SendMessage(packageToBroadcast);
                    await Task.Delay(1000);
                    packageNumber++;
                }
            });

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

        private async Task<IPAddress> GetBroacastAddressAsync(IPAddress ipAddress, IPAddress subnetMask)
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
                    broadcastAddress[i] = (byte)(ipAddressBytes[i] | subnetMaskBytes[i] ^ 255);
                }
                return new IPAddress(broadcastAddress);
            });
        }

        public void StopBroadcast()
        {
            serverRunning = false;
        }

        public void Dispose()
        {
            StopBroadcast();
        }
    }
}

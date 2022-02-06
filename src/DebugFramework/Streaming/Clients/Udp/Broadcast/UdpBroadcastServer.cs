using DebugFramework.DataTypes;
using DebugFramework.Streaming.Clients.Udp.Communication;
using DebugFramework.Streaming.Package;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Udp.Broadcast
{
    /// <summary>
    /// Broadcast server based on the udp protocol
    /// </summary>
    /// <typeparam name="T">The data type which should be broadcasted</typeparam>
    public class UdpBroadcastServer<T> : BaseNetworkClient, IDisposable where T : BaseDataType
    {
        /// <summary>
        /// Is the server running right now
        /// </summary>
        private bool serverRunning;

        /// <summary>
        /// The current package number used for sending
        /// </summary>
        private uint packageNumber;

        /// <summary>
        /// Is the broadcast data locked right now
        /// </summary>
        private object broadcastDataLock;

        /// <summary>
        /// The boradcast data to send
        /// </summary>
        private T broadcastData;

        /// <summary>
        /// Is the broadcast client locked right now
        /// </summary>
        private object broadcastClientLock;

        /// <summary>
        /// The endpoint of the boradcast to send to
        /// </summary>
        private readonly IPEndPoint broadcastEndPoint;

        /// <summary>
        /// The udp client to use for send the broadcast data
        /// </summary>
        private readonly UdpSendClient broadcastClient;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="broadcastPort">The port used for sending the broadcast around</param>
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

        /// <summary>
        /// Start the broadcast Server
        /// </summary>
        /// <param name="packageToBroadcast">The package to broadcast around</param>
        /// <param name="broadcastData">The data to broadcast with the package</param>
        /// <returns>A awaitable task which will end after you stopped the server</returns>
        public async Task StartBroadcastAsync(UdpPackage packageToBroadcast, T broadcastData)
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
                            SendBroadcast(packageToBroadcast);
                        }
                        await Task.Delay(1000);
                        packageNumber++;
                    }
                }
            });
        }

        /// <summary>
        /// Send a broadcast package to the broadcast endpoint
        /// </summary>
        /// <param name="packageToBroadcast">The package to broadcast</param>
        public void SendBroadcast(UdpPackage packageToBroadcast)
        {
            broadcastClient.SendTo(broadcastEndPoint, packageToBroadcast);
        }

        /// <summary>
        /// Change the data to broadcast
        /// </summary>
        /// <param name="newData">The new data to use for broadcasting</param>
        /// <returns>Was changing the data successful</returns>
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

        /// <summary>
        /// Get the broadcast address for this local machine
        /// </summary>
        /// <param name="ipAddress">The ip address to get the broadcast address for</param>
        /// <param name="subnetMask">The subnet mask to use</param>
        /// <returns>The ip address to send the broadcast to</returns>
        private IPAddress GetBroadcastAddress(IPAddress ipAddress, IPAddress subnetMask)
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
        }

        /// <summary>
        /// Stop the broadcasting of the message
        /// </summary>
        public void StopBroadcast()
        {
            serverRunning = false;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            StopBroadcast();
            broadcastClient.Dispose();
        }
    }
}

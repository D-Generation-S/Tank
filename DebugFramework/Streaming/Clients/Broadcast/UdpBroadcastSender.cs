using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Broadcast
{
    public class UdpBroadcastSender<T> : BroadcastClient, IDisposable where T : BaseDataType
    {
        private readonly IPEndPoint broadcastEndpoint;
        private readonly Socket broadcastSocket;

        public UdpBroadcastSender(int broadcastPort)
        {
            UnicastIPAddressInformation unicastInformation = GetUnicastAdressInformation();
            IPAddress broadcastAddress = GetBroadcastAddress(unicastInformation.Address, unicastInformation.IPv4Mask);
            broadcastEndpoint = new IPEndPoint(broadcastAddress, broadcastPort);
            broadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public async Task SendMessageAsync(UdpPackage<T> dataPackage)
        {
            await Task.Run(() => SendMessage(dataPackage));
        }

        public void SendMessage(UdpPackage<T> dataPackage)
        {
            byte[] dataToSend = dataPackage.GetDataStream();
            broadcastSocket.SendTo(dataToSend, broadcastEndpoint);
        }


        public void Dispose()
        {

        }
    }
}

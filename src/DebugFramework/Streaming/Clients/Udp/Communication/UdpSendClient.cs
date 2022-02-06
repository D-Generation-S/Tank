using DebugFramework.Streaming.Package;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Udp.Communication
{
    /// <summary>
    /// Udp class to send data to a ip address and port combination
    /// </summary>
    public class UdpSendClient : BaseUdpCommunicationClient, INetworkSendClient
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public UdpSendClient() : base()
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="defaultTargetIp">The default ip address to send data to</param>
        /// <param name="defaultTargetPort">The default port to send data to</param>
        public UdpSendClient(IPAddress defaultTargetIp, int defaultTargetPort) : base(defaultTargetIp, defaultTargetPort)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="defaultTargetIp">The default ip address to send data to</param>
        /// <param name="defaultTargetPort">The default port to send data to</param>
        /// <param name="clientToUse">The client to use for sending</param>
        public UdpSendClient(IPAddress defaultTargetIp, int defaultTargetPort, UdpClient clientToUse)
            : base(clientToUse, new IPEndPoint(defaultTargetIp, defaultTargetPort))
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="clientToUse">The client to use for sending</param>
        public UdpSendClient(UdpClient clientToUse) : base(clientToUse)
        {
        }

        /// <inheritdoc/>
        public async Task SendMessageAsync(IDataPackage dataPackage)
        {
            await Task.Run(() => SendMessage(dataPackage));
        }

        /// <inheritdoc/>
        public void SendMessage(IDataPackage udpPackage)
        {
            if (usedEndpoint == null)
            {
                return;
            }
            SendTo(usedEndpoint, udpPackage);
        }

        /// <inheritdoc/>
        public void SendTo(CommunicationPackage communicationPackage)
        {
            SendTo(communicationPackage.Sender, communicationPackage.dataPackage);
        }

        /// <inheritdoc/>
        public void SendTo(IPEndPoint endPoint, IDataPackage udpPackage)
        {
            byte[] dataToSend = udpPackage.GetDataStream();
            communicationClient.Send(dataToSend, dataToSend.Length, endPoint);
        }
    }
}

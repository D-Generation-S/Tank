using System;
using System.Net;
using System.Net.Sockets;

namespace DebugFramework.Streaming.Clients.Udp.Communication
{
    public class BaseUdpCommunicationClient : BaseUdpClient, IDisposable
    {
        protected readonly UdpClient communicationClient;
        protected readonly IPEndPoint usedEndpoint;

        public BaseUdpCommunicationClient() : this(new UdpClient())
        {
        }

        public BaseUdpCommunicationClient(IPAddress listenIp, int listenPort)
            : this(new UdpClient(listenPort, AddressFamily.InterNetwork), new IPEndPoint(listenIp, listenPort))
        {

        }

        public BaseUdpCommunicationClient(UdpClient clientToUse) : this(clientToUse, null)
        {
        }

        protected BaseUdpCommunicationClient(UdpClient clientToUse, IPEndPoint endpoint)
        {
            communicationClient = clientToUse;
            usedEndpoint = endpoint;
        }

        public void Dispose()
        {
            communicationClient.Dispose();
        }
    }


}

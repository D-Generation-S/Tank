using DebugFramework.DataTypes;
using System.Net;
using System.Net.Sockets;

namespace DebugFramework.Streaming.Clients.Communication
{
    public class BaseUdpCommunicationClient<T> : BaseUdpClient where T : BaseDataType
    {
        protected readonly UdpClient communicationClient;
        protected readonly IPEndPoint usedEndpoint;

        public BaseUdpCommunicationClient()
        {
            communicationClient = new UdpClient();
        }

        public BaseUdpCommunicationClient(IPAddress listenIp, int listenPort)
        {
            communicationClient = new UdpClient(listenPort, AddressFamily.InterNetwork);
            usedEndpoint = new IPEndPoint(listenIp, listenPort);
        }
    }


}

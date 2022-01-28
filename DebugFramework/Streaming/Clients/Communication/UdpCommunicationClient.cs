using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public class UdpCommunicationClient<T> : BaseUdpClient, IUdpCommunicationClient<T> where T : BaseDataType
    {
        private readonly UdpRecieveClient<T> recieveClient;
        private readonly UdpSendClient<T> sendClient;

        public UdpCommunicationClient(IPAddress listenIp)
        {
            int portToUse = GetFreePort(1024, 49150);
            UdpClient udpClient = new UdpClient(portToUse, AddressFamily.InterNetwork);

            recieveClient = new UdpRecieveClient<T>(listenIp, portToUse, udpClient);
            sendClient = new UdpSendClient<T>(udpClient);
        }

        public UdpCommunicationClient(IPAddress listenIp, int listenAndSendPort)
            : this(listenIp, listenAndSendPort, true)
        {
            UdpClient udpClient = new UdpClient(listenAndSendPort, AddressFamily.InterNetwork);

            recieveClient = new UdpRecieveClient<T>(listenIp, listenAndSendPort, udpClient);
            sendClient = new UdpSendClient<T>(udpClient);
        }

        public UdpCommunicationClient(IPAddress listenIp, int listenPort, bool sameSendPort)
        {
            UdpClient udpClient = new UdpClient(listenPort, AddressFamily.InterNetwork);

            recieveClient = new UdpRecieveClient<T>(listenIp, listenPort, udpClient);
            sendClient = sameSendPort ? new UdpSendClient<T>(udpClient) : new UdpSendClient<T>(new UdpClient(GetFreePort(1024, 49150), AddressFamily.InterNetwork));
        }

        public UdpCommunicationClient(IPAddress listenIp, int listenPort, int sendPort)
        {
            UdpClient listenClient = new UdpClient(listenPort, AddressFamily.InterNetwork);
            UdpClient sendClientToUse = new UdpClient(sendPort, AddressFamily.InterNetwork);

            recieveClient = new UdpRecieveClient<T>(listenIp, listenPort, listenClient);
            sendClient = new UdpSendClient<T>(sendClientToUse);
        }

        public CommunicationPackage<T> RecieveCommunicationPackage()
        {
            return recieveClient?.RecieveCommunicationPackage();
        }

        public async Task<CommunicationPackage<T>> RecieveCommunicationPackageAsync()
        {
            return await recieveClient?.RecieveCommunicationPackageAsync();
        }

        public T RecieveMessage()
        {
            return recieveClient?.RecieveMessage();
        }

        public async Task<T> RecieveMessageAsync()
        {
            return await recieveClient?.RecieveMessageAsync();
        }

        public void SendMessage(UdpPackage<T> udpPackage)
        {
            sendClient?.SendMessage(udpPackage);
        }

        public async Task SendMessageAsync(UdpPackage<T> dataPackage)
        {
            await sendClient.SendMessageAsync(dataPackage);
        }

        public void SendTo(CommunicationPackage<T> communicationPackage)
        {
            sendClient.SendTo(communicationPackage);
        }

        public void SendTo(IPEndPoint endPoint, UdpPackage<T> udpPackage)
        {
            sendClient.SendTo(endPoint, udpPackage);
        }
    }
}

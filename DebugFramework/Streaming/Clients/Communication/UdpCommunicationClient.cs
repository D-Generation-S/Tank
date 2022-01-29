using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public class UdpCommunicationClient : BaseUdpClient, IUdpCommunicationClient
    {
        private readonly UdpRecieveClient recieveClient;
        private readonly UdpSendClient sendClient;

        public UdpCommunicationClient(IPAddress listenIp)
        {
            int portToUse = GetFreePort(1024, 49150);
            UdpClient udpClient = new UdpClient(portToUse, AddressFamily.InterNetwork);

            recieveClient = new UdpRecieveClient(listenIp, portToUse, udpClient);
            sendClient = new UdpSendClient(udpClient);
        }

        public UdpCommunicationClient(IPAddress listenIp, int listenAndSendPort)
            : this(listenIp, listenAndSendPort, true)
        {
            UdpClient udpClient = new UdpClient(listenAndSendPort, AddressFamily.InterNetwork);

            recieveClient = new UdpRecieveClient(listenIp, listenAndSendPort, udpClient);
            sendClient = new UdpSendClient(udpClient);
        }

        public UdpCommunicationClient(IPAddress listenIp, int listenPort, bool sameSendPort)
        {
            UdpClient udpClient = new UdpClient(listenPort, AddressFamily.InterNetwork);

            recieveClient = new UdpRecieveClient(listenIp, listenPort, udpClient);
            sendClient = sameSendPort ? new UdpSendClient(udpClient) : new UdpSendClient(new UdpClient(GetFreePort(1024, 49150), AddressFamily.InterNetwork));
        }

        public UdpCommunicationClient(IPAddress listenIp, int listenPort, int sendPort)
        {
            UdpClient listenClient = new UdpClient(listenPort, AddressFamily.InterNetwork);
            UdpClient sendClientToUse = new UdpClient(sendPort, AddressFamily.InterNetwork);

            recieveClient = new UdpRecieveClient(listenIp, listenPort, listenClient);
            sendClient = new UdpSendClient(sendClientToUse);
        }

        public void Dispose()
        {
            sendClient?.Dispose();
            recieveClient?.Dispose();
        }

        public CommunicationPackage RecieveCommunicationPackage()
        {
            return recieveClient?.RecieveCommunicationPackage();
        }

        public async Task<CommunicationPackage> RecieveCommunicationPackageAsync()
        {
            return await recieveClient?.RecieveCommunicationPackageAsync();
        }

        public BaseDataType RecieveMessage()
        {
            return recieveClient?.RecieveMessage();
        }

        public async Task<BaseDataType> RecieveMessageAsync()
        {
            return await recieveClient?.RecieveMessageAsync();
        }

        public void SendMessage(UdpPackage udpPackage)
        {
            sendClient?.SendMessage(udpPackage);
        }

        public async Task SendMessageAsync(UdpPackage dataPackage)
        {
            await sendClient.SendMessageAsync(dataPackage);
        }

        public void SendTo(CommunicationPackage communicationPackage)
        {
            sendClient.SendTo(communicationPackage);
        }

        public void SendTo(IPEndPoint endPoint, UdpPackage udpPackage)
        {
            sendClient.SendTo(endPoint, udpPackage);
        }
    }
}

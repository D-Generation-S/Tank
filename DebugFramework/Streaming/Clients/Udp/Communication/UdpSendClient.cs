using DebugFramework.Streaming.Clients.Communication;
using DebugFramework.Streaming.Package;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Udp.Communication
{
    public class UdpSendClient : BaseUdpCommunicationClient, IUdpSendClient
    {
        public UdpSendClient() : base()
        {

        }
        public UdpSendClient(IPAddress defaultTargetIp, int defaultTargetPort) : base(defaultTargetIp, defaultTargetPort)
        {
        }

        public UdpSendClient(IPAddress defaultTargetIp, int defaultTargetPort, UdpClient clientToUse)
            : base(clientToUse, new IPEndPoint(defaultTargetIp, defaultTargetPort))
        {
        }

        public UdpSendClient(UdpClient clientToUse) : base(clientToUse)
        {
        }

        public async Task SendMessageAsync(UdpPackage dataPackage)
        {
            await Task.Run(() => SendMessage(dataPackage));
        }

        public void SendMessage(UdpPackage udpPackage)
        {
            if (usedEndpoint == null)
            {
                return;
            }
            SendTo(usedEndpoint, udpPackage);
        }

        public void SendTo(CommunicationPackage communicationPackage)
        {
            SendTo(communicationPackage.Sender, communicationPackage.UdpPackage);
        }

        public void SendTo(IPEndPoint endPoint, UdpPackage udpPackage)
        {
            byte[] dataToSend = udpPackage.GetDataStream();
            communicationClient.Send(dataToSend, dataToSend.Length, endPoint);
        }

        public void Dispose()
        {
            base.Dispose();
        }
    }
}

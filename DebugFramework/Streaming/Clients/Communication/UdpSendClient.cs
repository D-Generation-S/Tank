using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System.Net;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public class UdpSendClient<T> : BaseUdpCommunicationClient<T> where T : BaseDataType
    {
        public UdpSendClient() : base()
        {

        }
        public UdpSendClient(IPAddress defaultTargetIp, int defaultTargetPort) : base(defaultTargetIp, defaultTargetPort)
        {
        }

        public async Task SendMessageAsync(UdpPackage<T> dataPackage)
        {
            await Task.Run(() => SendMessage(dataPackage));
        }

        public void SendMessage(UdpPackage<T> udpPackage)
        {
            if (usedEndpoint == null)
            {
                return;
            }
            SendTo(usedEndpoint, udpPackage);
        }

        public void SendTo(CommunicationPackage<T> communicationPackage)
        {
            SendTo(communicationPackage.Sender, communicationPackage.UdpPackage);
        }

        public void SendTo(IPEndPoint endPoint, UdpPackage<T> udpPackage)
        {
            byte[] dataToSend = udpPackage.GetDataStream();
            communicationClient.Send(dataToSend, dataToSend.Length, endPoint);
        }


    }
}

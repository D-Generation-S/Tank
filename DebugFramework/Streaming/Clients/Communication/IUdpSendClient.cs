using DebugFramework.Streaming.Package;
using System.Net;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public interface IUdpSendClient
    {
        void SendMessage(UdpPackage udpPackage);
        Task SendMessageAsync(UdpPackage dataPackage);
        void SendTo(CommunicationPackage communicationPackage);
        void SendTo(IPEndPoint endPoint, UdpPackage udpPackage);
    }
}
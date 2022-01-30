using DebugFramework.Streaming.Clients.Udp.Communication;
using DebugFramework.Streaming.Package;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public interface IUdpSendClient : IDisposable
    {
        void SendMessage(UdpPackage udpPackage);
        Task SendMessageAsync(UdpPackage dataPackage);
        void SendTo(CommunicationPackage communicationPackage);
        void SendTo(IPEndPoint endPoint, UdpPackage udpPackage);
    }
}
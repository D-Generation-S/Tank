using DebugFramework.Streaming.Clients.Udp.Communication;
using DebugFramework.Streaming.Package;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public interface INetworkSendClient : IDisposable
    {
        void SendMessage(IDataPackage udpPackage);
        Task SendMessageAsync(IDataPackage dataPackage);
        void SendTo(CommunicationPackage communicationPackage);
        void SendTo(IPEndPoint endPoint, IDataPackage udpPackage);
    }
}
using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System.Net;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public interface IUdpSendClient<T> where T : BaseDataType
    {
        void SendMessage(UdpPackage<T> udpPackage);
        Task SendMessageAsync(UdpPackage<T> dataPackage);
        void SendTo(CommunicationPackage<T> communicationPackage);
        void SendTo(IPEndPoint endPoint, UdpPackage<T> udpPackage);
    }
}
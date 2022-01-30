using DebugFramework.DataTypes;
using DebugFramework.Streaming.Clients.Udp.Communication;
using System;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public interface INetworkRecieveClient : IDisposable
    {
        CommunicationPackage RecieveCommunicationPackage();
        Task<CommunicationPackage> RecieveCommunicationPackageAsync();
        BaseDataType RecieveMessage();
        Task<BaseDataType> RecieveMessageAsync();
    }
}
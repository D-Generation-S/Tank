using DebugFramework.DataTypes;
using System;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public interface IUdpRecieveClient : IDisposable
    {
        CommunicationPackage RecieveCommunicationPackage();
        Task<CommunicationPackage> RecieveCommunicationPackageAsync();
        BaseDataType RecieveMessage();
        Task<BaseDataType> RecieveMessageAsync();
    }
}
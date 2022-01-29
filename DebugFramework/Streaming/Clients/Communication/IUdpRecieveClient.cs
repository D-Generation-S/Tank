using DebugFramework.DataTypes;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public interface IUdpRecieveClient
    {
        CommunicationPackage RecieveCommunicationPackage();
        Task<CommunicationPackage> RecieveCommunicationPackageAsync();
        BaseDataType RecieveMessage();
        Task<BaseDataType> RecieveMessageAsync();
    }
}
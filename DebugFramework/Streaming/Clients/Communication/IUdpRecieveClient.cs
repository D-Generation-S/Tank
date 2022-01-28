using DebugFramework.DataTypes;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public interface IUdpRecieveClient<T> where T : BaseDataType
    {
        CommunicationPackage<T> RecieveCommunicationPackage();
        Task<CommunicationPackage<T>> RecieveCommunicationPackageAsync();
        T RecieveMessage();
        Task<T> RecieveMessageAsync();
    }
}
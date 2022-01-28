using DebugFramework.DataTypes;

namespace DebugFramework.Streaming.Clients.Communication
{
    public interface IUdpCommunicationClient<T> : IUdpRecieveClient<T>, IUdpSendClient<T> where T : BaseDataType
    {
    }
}

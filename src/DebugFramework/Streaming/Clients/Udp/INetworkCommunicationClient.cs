using DebugFramework.Streaming.Clients.Communication;

namespace DebugFramework.Streaming.Clients.Udp
{
    /// <summary>
    /// Interface for a client which communicates into both directions
    /// </summary>
    public interface INetworkCommunicationClient : INetworkRecieveClient, INetworkSendClient
    {
    }
}

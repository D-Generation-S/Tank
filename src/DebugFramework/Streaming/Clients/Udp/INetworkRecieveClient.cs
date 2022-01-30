using DebugFramework.DataTypes;
using DebugFramework.Streaming.Clients.Udp.Communication;
using System;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    /// <summary>
    /// Interface for a network client which can only recieve messages
    /// </summary>
    public interface INetworkRecieveClient : IDisposable
    {
        /// <summary>
        /// Get a communication package from the network client
        /// </summary>
        /// <returns>A communication package</returns>
        CommunicationPackage RecieveCommunicationPackage();

        /// <summary>
        /// Get a communication asynchronous package from the network client
        /// </summary>
        /// <returns>A communication package task to wait for</returns>
        Task<CommunicationPackage> RecieveCommunicationPackageAsync();

        /// <summary>
        /// Recieve a base data type message
        /// </summary>
        /// <returns>A base data type</returns>
        BaseDataType RecieveMessage();

        /// <summary>
        /// Recieve a base data type message asynchronous
        /// </summary>
        /// <returns>A base data type</returns>
        Task<BaseDataType> RecieveMessageAsync();
    }
}
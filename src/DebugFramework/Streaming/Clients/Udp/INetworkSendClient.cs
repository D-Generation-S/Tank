using DebugFramework.Streaming.Clients.Udp.Communication;
using DebugFramework.Streaming.Package;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Udp
{
    /// <summary>
    /// Interface for clients which can only send data
    /// </summary>
    public interface INetworkSendClient : IDisposable
    {
        /// <summary>
        /// Send a message to the default endpoint
        /// </summary>
        /// <param name="udpPackage">The data package to send</param>
        void SendMessage(IDataPackage udpPackage);

        /// <summary>
        /// Send a message to the default endpoins asynchronous
        /// </summary>
        /// <param name="dataPackage">The data package to send</param>
        /// <returns></returns>
        Task SendMessageAsync(IDataPackage dataPackage);

        /// <summary>
        /// Send a message to a specific endpoint
        /// </summary>
        /// <param name="communicationPackage">The communication package with all the information required for sending</param>
        void SendTo(CommunicationPackage communicationPackage);

        /// <summary>
        /// Send a message to a specific endpoint
        /// </summary>
        /// <param name="endPoint">The endpoint to send the message to</param>
        /// <param name="udpPackage">The data to send</param>
        void SendTo(IPEndPoint endPoint, IDataPackage udpPackage);
    }
}
using System;
using System.Net;
using System.Net.Sockets;

namespace DebugFramework.Streaming.Clients.Udp.Communication
{
    /// <summary>
    /// Class to define a communcation udp client and a endpoint to use
    /// </summary>
    public class BaseUdpCommunicationClient : BaseNetworkClient, IDisposable
    {
        /// <summary>
        /// The communcation client to use
        /// </summary>
        protected readonly UdpClient communicationClient;

        /// <summary>
        /// The endpoint used for sending data to
        /// </summary>
        protected readonly IPEndPoint usedEndpoint;

        /// <summary>
        /// Create a new instance of this class with a random udp client
        /// </summary>
        public BaseUdpCommunicationClient() : this(new UdpClient()) { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="listenIp">The ip address to listen to</param>
        /// <param name="listenPort">The port to listen on</param>
        public BaseUdpCommunicationClient(IPAddress listenIp, int listenPort)
            : this(new UdpClient(listenPort, AddressFamily.InterNetwork), new IPEndPoint(listenIp, listenPort)) { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="clientToUse">The cline to use for listening</param>
        public BaseUdpCommunicationClient(UdpClient clientToUse) : this(clientToUse, null) { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="clientToUse">The cline to use for listening</param>
        /// <param name="endpoint">The endpoint to send data to or recieve from</param>
        protected BaseUdpCommunicationClient(UdpClient clientToUse, IPEndPoint endpoint)
        {
            communicationClient = clientToUse;
            usedEndpoint = endpoint;
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            communicationClient.Dispose();
        }
    }


}

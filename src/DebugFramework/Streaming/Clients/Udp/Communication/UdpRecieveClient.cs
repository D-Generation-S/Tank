using DebugFramework.DataTypes;
using DebugFramework.Streaming.Clients.Communication;
using DebugFramework.Streaming.Package;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Udp.Communication
{
    /// <summary>
    /// Udp client to recieve data
    /// </summary>
    public class UdpRecieveClient : BaseUdpCommunicationClient, INetworkRecieveClient
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public UdpRecieveClient() { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="listenPort">The port to listen on</param>
        public UdpRecieveClient(int listenPort) : this(IPAddress.Any, listenPort) { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="listenIp">The ip address to listen for</param>
        /// <param name="listenPort">The port to listen on</param>
        public UdpRecieveClient(IPAddress listenIp, int listenPort) : base(listenIp, listenPort) { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="listenIp">The ip address to listen for</param>
        /// <param name="listenPort">The port to listen on</param>
        /// <param name="clientToUse">The client to use for listening</param>
        public UdpRecieveClient(IPAddress listenIp, int listenPort, UdpClient clientToUse) : base(clientToUse, new IPEndPoint(listenIp, listenPort)) { }

        /// <inheritdoc/>
        public BaseDataType RecieveMessage()
        {
            return RecieveCommunicationPackage().dataPackage?.GetPayload();
        }

        /// <summary>
        /// Recieve a message of a given type T
        /// </summary>
        /// <typeparam name="T">The type to cast the message to</typeparam>
        /// <returns>The message of type T</returns>
        public T RecieveMessage<T>() where T : BaseDataType => RecieveCommunicationPackage().dataPackage?.GetPayload<T>();

        /// <inheritdoc/>
        public async Task<BaseDataType> RecieveMessageAsync()
        {
            CommunicationPackage returnPackage = await RecieveCommunicationPackageAsync();
            return returnPackage.dataPackage.GetPayload();
        }

        /// <summary>
        /// Recieve a message of a given type T async
        /// </summary>
        /// <typeparam name="T">The type to cast the message to</typeparam>
        /// <returns>The message of type T</returns>
        public async Task<T> RecieveMessageAsync<T>() where T : BaseDataType
        {
            CommunicationPackage returnPackage = await RecieveCommunicationPackageAsync();
            return returnPackage.dataPackage.GetPayload<T>();
        }

        /// <inheritdoc/>
        public CommunicationPackage RecieveCommunicationPackage()
        {
            if (usedEndpoint == null)
            {
                return default;
            }
            IPEndPoint recieveEndpoint = usedEndpoint;
            UdpPackage recievedPackage = new UdpPackage();
            byte[] recievedData = communicationClient.Receive(ref recieveEndpoint);
            recievedPackage.Init(recievedData);
            return new CommunicationPackage(usedEndpoint, recievedPackage);
        }

        /// <inheritdoc/>
        public async Task<CommunicationPackage> RecieveCommunicationPackageAsync()
        {
            return await Task.Run(() =>
            {
                IPEndPoint recieveEndpoint = usedEndpoint;
                if (recieveEndpoint == null)
                {
                    return null;
                }
                CommunicationPackage communicationPackage = null;
                UdpPackage udpPackage = new UdpPackage();
                while (!udpPackage.IsPackageComplete() || !udpPackage.IsPayloadFine())
                {
                    byte[] recievedData = communicationClient.Receive(ref recieveEndpoint);
                    udpPackage.Init(recievedData);
                    communicationPackage = new CommunicationPackage(usedEndpoint, udpPackage);
                }
                return communicationPackage;
            });
        }
    }
}

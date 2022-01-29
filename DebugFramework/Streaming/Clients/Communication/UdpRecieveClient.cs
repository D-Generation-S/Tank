using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public class UdpRecieveClient : BaseUdpCommunicationClient, IUdpRecieveClient
    {
        public UdpRecieveClient()
        {

        }

        public UdpRecieveClient(int listenPort) : this(IPAddress.Any, listenPort)
        {
        }

        public UdpRecieveClient(IPAddress listenIp, int listenPort) : base(listenIp, listenPort)
        {
        }

        public UdpRecieveClient(IPAddress listenIp, int listenPort, UdpClient clientToUse) : base(clientToUse, new IPEndPoint(listenIp, listenPort))
        {
        }

        public BaseDataType RecieveMessage()
        {
            return RecieveCommunicationPackage().UdpPackage?.GetBasePayload();
        }

        public T RecieveMessage<T>() where T : BaseDataType
        {
            return RecieveCommunicationPackage().UdpPackage?.GetPayload<T>();
        }

        public async Task<BaseDataType> RecieveMessageAsync()
        {
            CommunicationPackage returnPackage = await RecieveCommunicationPackageAsync();
            return returnPackage.UdpPackage.GetBasePayload();
        }

        public async Task<T> RecieveMessageAsync<T>() where T : BaseDataType
        {
            CommunicationPackage returnPackage = await RecieveCommunicationPackageAsync();
            return returnPackage.UdpPackage.GetPayload<T>();
        }

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
                while (!udpPackage.PackageIsComplete() || !udpPackage.PayloadIsFine())
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

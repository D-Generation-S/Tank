using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System.Net;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Communication
{
    public class UdpRecieveClient<T> : BaseUdpCommunicationClient<T> where T : BaseDataType
    {
        //public UdpRecieveClient() : base()
        //{
        //}

        public UdpRecieveClient(int listenPort) : this(IPAddress.Any, listenPort)
        {
        }

        public UdpRecieveClient(IPAddress listenIp, int listenPort) : base(listenIp, listenPort)
        {
        }

        public T RecieveMessage()
        {
            return RecieveCommunicationPackage().UdpPackage?.GetPayload<T>();
        }

        public CommunicationPackage<T> RecieveCommunicationPackage()
        {
            if (usedEndpoint == null)
            {
                return default;
            }
            IPEndPoint recieveEndpoint = usedEndpoint;
            UdpPackage<T> recievedPackage = new UdpPackage<T>();
            byte[] recievedData = communicationClient.Receive(ref recieveEndpoint);
            recievedPackage.Init(recievedData);
            return new CommunicationPackage<T>(usedEndpoint, recievedPackage);
        }

        public async Task<CommunicationPackage<T>> RecieveCommunicationPackageAsync()
        {
            return await Task.Run(() =>
            {
                IPEndPoint recieveEndpoint = usedEndpoint;
                if (recieveEndpoint == null)
                {
                    return null;
                }
                CommunicationPackage<T> communicationPackage = null;
                UdpPackage<T> udpPackage = new UdpPackage<T>();
                while (!udpPackage.PackageIsComplete() || !udpPackage.PayloadIsFine())
                {
                    byte[] recievedData = communicationClient.Receive(ref recieveEndpoint);
                    udpPackage.Init(recievedData);
                    communicationPackage = new CommunicationPackage<T>(usedEndpoint, udpPackage);
                }
                return communicationPackage;
            });
        }


        public async Task<T> RecieveMessageAsync()
        {
            CommunicationPackage<T> returnPackage = await RecieveCommunicationPackageAsync();
            return returnPackage.UdpPackage.GetPayload<T>();
        }
        /**
        public T RecieveMessageFrom(IPEndPoint endPoint)
        {
            return RecieveCommunicationPackageFrom(endPoint).UdpPackage?.GetPayload<T>();
        }

        public async Task<T> RecieveMessageAsyncFrom(IPEndPoint endPoint)
        {
            CommunicationPackage<T> returnPackage = await RecieveCommunicationPackageFromAsync(endPoint);
            return returnPackage.UdpPackage.GetPayload<T>();
        }

        public CommunicationPackage<T> RecieveCommunicationPackageFrom(IPEndPoint endPoint)
        {
            if (endPoint == null)
            {
                return default;
            }
            UdpPackage<T> recievedPackage = new UdpPackage<T>();
            byte[] recievedData = communicationClient.Receive(ref endPoint);
            recievedPackage.Init(recievedData);
            return new CommunicationPackage<T>(usedEndpoint, recievedPackage);
        }

        public async Task<CommunicationPackage<T>> RecieveCommunicationPackageFromAsync(IPEndPoint endPoint)
        {
            return await Task.Run(() =>
            {
                if (endPoint == null)
                {
                    return null;
                }
                CommunicationPackage<T> communicationPackage = null;
                UdpPackage<T> udpPackage = new UdpPackage<T>();
                while (!udpPackage.PackageIsComplete() || !udpPackage.PayloadIsFine())
                {
                    byte[] recievedData = communicationClient.Receive(ref endPoint);
                    udpPackage.Init(recievedData);
                    communicationPackage = new CommunicationPackage<T>(usedEndpoint, udpPackage);
                }
                return communicationPackage;
            });
        }
        */
    }
}

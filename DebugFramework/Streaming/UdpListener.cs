using DebugFramework.DataTypes;
using DebugFramework.DataTypes.Responses;
using DebugFramework.Streaming.Package;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming
{
    public class UdpListener
    {
        private UdpClient broadcastListener;
        private IPEndPoint broadcastEndpoint;

        private UdpClient updateListener;
        private IPEndPoint updateEndpoint;

        private object packageNumberLock = new object();

        private bool updateListnerConnected;
        private uint packageNumber;

        public async Task<BroadcastData> ListenForBroadcastAsync()
        {
            broadcastListener = broadcastListener ?? new UdpClient(Configuration.BROADCAST_IP, AddressFamily.InterNetwork);
            broadcastEndpoint = broadcastEndpoint ?? new IPEndPoint(IPAddress.Any, Configuration.BROADCAST_IP);
            return await Task.Run(() =>
            {
                UdpPackage<BroadcastData> udpPackage = new UdpPackage<BroadcastData>();
                while (!udpPackage.PackageIsComplete() || !udpPackage.PayloadIsFine())
                {
                    byte[] recievedData = broadcastListener.Receive(ref broadcastEndpoint);
                    udpPackage.Init(recievedData);
                }

                return udpPackage.GetPayload<BroadcastData>();
            });
        }

        public async Task<BaseDataType> ListenForUpdatesAsync(string IpAddress, int port, Func<UdpPackage<BaseDataType>, Type, BaseDataType> conversionMethod)
        {
            updateListener = updateListener ?? new UdpClient(port);
            updateEndpoint = updateEndpoint ?? new IPEndPoint(IPAddress.Parse(IpAddress), port);

            return await Task.Run(async () =>
            {
                if (conversionMethod == null)
                {
                    return default;
                }
                await ConnectToUpdateServer(updateListener, updateEndpoint);
                BaseDataType returnData = new BaseDataType();
                UdpPackage<BaseDataType> basePackage = new UdpPackage<BaseDataType>();
                while (!basePackage.PackageIsComplete() || !basePackage.PayloadIsFine())
                {
                    byte[] recievedData = updateListener.Receive(ref updateEndpoint);
                    basePackage.Init(recievedData);
                    Type payloadType = Type.GetType(basePackage.GetBasePayload().AssemblyQualifiedName);
                    returnData = conversionMethod(basePackage, payloadType);
                }
                return returnData;
            });
        }

        public async Task<bool> ConnectToUpdateServer(UdpClient client, IPEndPoint endPoint)
        {
            return await Task.Run(() =>
            {
                UdpPackage<ConnectRequest> connectRequest = new UdpPackage<ConnectRequest>();
                lock (packageNumberLock)
                {
                    connectRequest.Init(packageNumber, DataIdentifier.Request, new ConnectRequest());
                    byte[] responseToSend = connectRequest.GetDataStream();
                    client.Send(responseToSend, responseToSend.Length, endPoint);
                    packageNumber++;
                }

                UdpPackage<RecievedConfirmation> confirmationPackage = new UdpPackage<RecievedConfirmation>();
                while (!confirmationPackage.PackageIsComplete() || !confirmationPackage.PayloadIsFine())
                {
                    byte[] recievedData = updateListener.Receive(ref updateEndpoint);
                    confirmationPackage.Init(recievedData);
                }

                return false;
            });
        }
    }
}

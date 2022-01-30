using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Tcp
{
    public class TcpRecieverClient : BaseNetworkClient, IDisposable
    {
        private readonly TcpClient recieverClient;
        private readonly IPEndPoint defaultEndpoint;

        private IPEndPoint currentEndpoint;

        public TcpRecieverClient(IPEndPoint defaultEndpoint)
        {
            recieverClient = new TcpClient();
            this.defaultEndpoint = defaultEndpoint;
        }

        private bool ConnectClient(IPEndPoint endpointToConnect)
        {
            if (endpointToConnect == currentEndpoint && recieverClient.Connected)
            {
                return true;
            }
            if (recieverClient.Connected)
            {
                recieverClient.Close();
            }
            currentEndpoint = endpointToConnect;
            try
            {
                recieverClient.Connect(endpointToConnect);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private TcpPackage RecieveDataPackage(IPEndPoint remoteAddress)
        {
            if (!recieverClient.Connected)
            {
                if (!ConnectClient(remoteAddress))
                {
                    return null;
                }
            }
            TcpPackage package = new TcpPackage();
            byte[] header = new byte[package.GetHeaderSize()];
            try
            {
                recieverClient.GetStream().Read(header, 0, header.Length);
            }
            catch (Exception)
            {
                return null;
            }
            if (package.ParseHeader(header))
            {
                byte[] packageData = new byte[package.GetCompletePackageSize()];
                recieverClient.GetStream().Read(packageData, package.GetHeaderSize(), package.GetPayloadSize());
                for (int i = 0; i < package.GetHeaderSize(); i++)
                {
                    packageData[i] = header[i];
                }
                package.Init(packageData);
            }

            return package;
        }

        public async Task<TcpPackage> RecieveDataPackageAsync() => await RecieveDataPackageAsync(defaultEndpoint);

        public async Task<TcpPackage> RecieveDataPackageAsync(IPEndPoint remoteAddress)
        {
            return await Task.Run(() => RecieveDataPackage(remoteAddress));
        }

        public BaseDataType RecievePackage() => RecievePackage(defaultEndpoint);

        public T RecievePackage<T>() where T : BaseDataType => RecievePackage<T>(defaultEndpoint);

        private bool PackageComplete(TcpPackage recievedPackage)
        {
            return recievedPackage.IsPackageComplete() && recievedPackage.IsPayloadFine();
        }

        public BaseDataType RecievePackage(IPEndPoint remoteAddress)
        {
            TcpPackage recievedPackage = RecieveDataPackage(remoteAddress);
            return PackageComplete(recievedPackage) ? recievedPackage.GetPayload() : null;
        }

        public T RecievePackage<T>(IPEndPoint remoteAddress) where T : BaseDataType
        {
            TcpPackage recievedPackage = RecieveDataPackage(remoteAddress);
            return PackageComplete(recievedPackage) ? recievedPackage.GetPayload<T>() : default(T);
        }

        public async Task<BaseDataType> RecievePackageAsync() => await RecievePackageAsync(defaultEndpoint);

        public async Task<T> RecievePackageAsync<T>() where T : BaseDataType => await RecievePackageAsync<T>(defaultEndpoint);

        public async Task<BaseDataType> RecievePackageAsync(IPEndPoint remoteAddress)
        {
            return await Task.Run(() =>
            {
                TcpPackage recievedPackage = RecieveDataPackage(remoteAddress);
                return PackageComplete(recievedPackage) ? recievedPackage.GetPayload() : null;
            });
        }

        public async Task<T> RecievePackageAsync<T>(IPEndPoint remoteAddress) where T : BaseDataType
        {
            return await Task.Run(() =>
            {
                TcpPackage recievedPackage = RecieveDataPackage(remoteAddress);
                return PackageComplete(recievedPackage) ? recievedPackage.GetPayload<T>() : default(T);
            });
        }

        public void Dispose()
        {
            if (recieverClient.Connected)
            {
                recieverClient.Close();
                recieverClient.Dispose();
            }
        }
    }
}

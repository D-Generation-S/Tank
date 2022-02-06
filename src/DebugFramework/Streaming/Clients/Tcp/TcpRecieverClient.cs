using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Tcp
{
    /// <summary>
    /// Class for a tcp reciever client (listner)
    /// </summary>
    public class TcpRecieverClient : BaseNetworkClient, IDisposable
    {
        /// <summary>
        /// The client to use for listening
        /// </summary>
        private readonly TcpClient recieverClient;

        /// <summary>
        /// The default endpoint used to connect to and get data from
        /// </summary>
        private readonly IPEndPoint defaultEndpoint;

        /// <summary>
        /// The current used endpoint
        /// </summary>
        private IPEndPoint currentEndpoint;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="defaultEndpoint">The default endpoint to use for the connection</param>
        public TcpRecieverClient(IPEndPoint defaultEndpoint)
        {
            recieverClient = new TcpClient();
            this.defaultEndpoint = defaultEndpoint;
        }

        /// <summary>
        /// Connect the client to a specific endpoint
        /// </summary>
        /// <param name="endpointToConnect">The endpoint to connect the client to</param>
        /// <returns>True if connection was successful</returns>
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

        /// <summary>
        /// Recieve a tcp package from a given address
        /// </summary>
        /// <param name="remoteAddress">The remote address to recieve data from</param>
        /// <returns>A tcp package to use</returns>
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

        /// <summary>
        /// Recieve a tcp package from a given address asynchronous
        /// </summary>
        /// <returns>A tcp package to wait for</returns>
        public async Task<TcpPackage> RecieveDataPackageAsync() => await RecieveDataPackageAsync(defaultEndpoint);

        /// <summary>
        /// Recieve a tcp package from a given address asynchronous
        /// </summary>
        /// <param name="remoteAddress">The specific endpoint to get the data package from</param>
        /// <returns>A tcp package to wait for</returns>
        public async Task<TcpPackage> RecieveDataPackageAsync(IPEndPoint remoteAddress)
        {
            return await Task.Run(() => RecieveDataPackage(remoteAddress));
        }

        /// <summary>
        /// Recieve a package from the default endpoint
        /// </summary>
        /// <returns>The base data type recieved by the client</returns>
        public BaseDataType RecievePackage() => RecievePackage(defaultEndpoint);

        /// <summary>
        /// Recieve a package of type T from a given endponts
        /// </summary>
        /// <typeparam name="T">The endpoint to get the package from</typeparam>
        /// <returns>The package of type T</returns>
        public T RecievePackage<T>() where T : BaseDataType => RecievePackage<T>(defaultEndpoint);

        /// <summary>
        /// Is the current package compoete
        /// </summary>
        /// <param name="recievedPackage">Package to check for completion</param>
        /// <returns>True if the package is complete</returns>
        private bool PackageComplete(TcpPackage recievedPackage)
        {
            return recievedPackage.IsPackageComplete() && recievedPackage.IsPayloadFine();
        }

        /// <summary>
        /// Recieve a base data type package from a given endpoint
        /// </summary>
        /// <param name="remoteAddress">The endpoint to recieve the package from</param>
        /// <returns>The base data type of the package</returns>
        public BaseDataType RecievePackage(IPEndPoint remoteAddress)
        {
            TcpPackage recievedPackage = RecieveDataPackage(remoteAddress);
            return PackageComplete(recievedPackage) ? recievedPackage.GetPayload() : null;
        }

        /// <summary>
        /// Recieve a base data type package of type T from a given endpoint
        /// </summary>
        /// <param name="remoteAddress">The endpoint to recieve the package from</param>
        /// <returns>The base data type of type T of the package</returns>
        public T RecievePackage<T>(IPEndPoint remoteAddress) where T : BaseDataType
        {
            TcpPackage recievedPackage = RecieveDataPackage(remoteAddress);
            return PackageComplete(recievedPackage) ? recievedPackage.GetPayload<T>() : default(T);
        }

        /// <summary>
        /// Recieve a package async from the default endpoint
        /// </summary>
        /// <returns>The base data type recieved by the client</returns>
        public async Task<BaseDataType> RecievePackageAsync() => await RecievePackageAsync(defaultEndpoint);

        /// <summary>
        /// Recieve a package of type T async from a given endponts
        /// </summary>
        /// <typeparam name="T">The endpoint to get the package from</typeparam>
        /// <returns>The package of type T</returns>
        public async Task<T> RecievePackageAsync<T>() where T : BaseDataType => await RecievePackageAsync<T>(defaultEndpoint);

        /// <summary>
        /// Recieve a base data type package async from a given endpoint
        /// </summary>
        /// <param name="remoteAddress">The endpoint to recieve the package from</param>
        /// <returns>The base data type of the package</returns>
        public async Task<BaseDataType> RecievePackageAsync(IPEndPoint remoteAddress)
        {
            return await Task.Run(() =>
            {
                TcpPackage recievedPackage = RecieveDataPackage(remoteAddress);
                return PackageComplete(recievedPackage) ? recievedPackage.GetPayload() : null;
            });
        }

        /// <summary>
        /// Recieve a base data type package async of type T from a given endpoint
        /// </summary>
        /// <param name="remoteAddress">The endpoint to recieve the package from</param>
        /// <returns>The base data type of type T of the package</returns>
        public async Task<T> RecievePackageAsync<T>(IPEndPoint remoteAddress) where T : BaseDataType
        {
            return await Task.Run(() =>
            {
                TcpPackage recievedPackage = RecieveDataPackage(remoteAddress);
                return PackageComplete(recievedPackage) ? recievedPackage.GetPayload<T>() : default(T);
            });
        }

        /// <inheritdoc/>
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

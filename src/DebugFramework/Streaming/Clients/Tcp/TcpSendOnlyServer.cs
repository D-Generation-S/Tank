using DebugFramework.Streaming.Package;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Tcp
{
    /// <summary>
    /// Tcp client class which only can send data
    /// </summary>
    public class TcpSendOnlyServer : BaseNetworkClient, IDisposable
    {
        /// <summary>
        /// The tcp listner to use to establish connections
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// Do we listen for incomming conenctions right now
        /// </summary>
        private bool listenForConnections;

        /// <summary>
        /// Lock if a new client is getting added or removed
        /// </summary>
        private object connectedClientLock;

        /// <summary>
        /// All the clients which are connected right now
        /// </summary>
        private List<TcpClient> connectedClients;

        /// <summary>
        /// Do we send some data at the moment used for async send
        /// </summary>
        private bool sendingRightNow;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="port">The port to listen for incomming connection attemps</param>
        public TcpSendOnlyServer(int port)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            listener = new TcpListener(localEndPoint);

            connectedClients = new List<TcpClient>();
            connectedClientLock = new object();
        }

        /// <summary>
        /// Accept connections async, blocking if awaited for
        /// </summary>
        /// <returns>A task to await</returns>
        public async Task AcceptConnectionsAsync()
        {
            listenForConnections = true;
            await Task.Run(async () =>
              {
                  listener.Start();
                  while (listenForConnections)
                  {
                      TcpClient connectedClient = await listener.AcceptTcpClientAsync();
                      lock (connectedClientLock)
                      {
                          connectedClients.Add(connectedClient);
                      }
                  }
              });
            listener.Stop();
        }

        /// <summary>
        /// Send a tcp package to all connected clients
        /// </summary>
        /// <param name="tcpPackage">The tcp package to send</param>
        public void SendData(TcpPackage tcpPackage)
        {
            sendingRightNow = true;
            lock (connectedClientLock)
            {
                connectedClients.RemoveAll(client => !client.Connected);
                foreach (TcpClient client in connectedClients)
                {
                    if (client.Connected && client.GetStream().CanWrite)
                    {
                        byte[] dataToWrite = tcpPackage.GetDataStream();
                        Debug.WriteLine(dataToWrite.Length);
                        try
                        {
                            client.GetStream().Write(dataToWrite, 0, dataToWrite.Length);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            sendingRightNow = false;
        }

        /// <summary>
        /// Send data async
        /// </summary>
        /// <param name="tcpPackage">The tcp package to send</param>
        /// <returns></returns>
        public async Task SendDataAsync(TcpPackage tcpPackage)
        {
            if (sendingRightNow)
            {
                return;
            }
            await Task.Run(() => SendData(tcpPackage));
        }

        /// <summary>
        /// Stop to accept connections
        /// </summary>
        public void StopAcceptConnections()
        {
            listenForConnections = false;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            StopAcceptConnections();
            connectedClients.Clear();
            listener.Stop();

        }
    }
}

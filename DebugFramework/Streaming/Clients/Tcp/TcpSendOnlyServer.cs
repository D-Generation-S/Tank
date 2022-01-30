using DebugFramework.Streaming.Package;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DebugFramework.Streaming.Clients.Tcp
{
    public class TcpSendOnlyServer : BaseNetworkClient, IDisposable
    {
        private TcpListener listener;
        private bool listenForConnections;

        private object connectedClientLock;
        private List<TcpClient> connectedClients;

        private bool sendingRightNow;

        public TcpSendOnlyServer(int port)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            listener = new TcpListener(localEndPoint);

            connectedClients = new List<TcpClient>();
            connectedClientLock = new object();
        }

        public async Task AcceptConnectionsAsync()
        {
            listenForConnections = true;
            _ = Task.Run(async () =>
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

        public void SendData(TcpPackage tcpPackage)
        {
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
        }

        public async Task SendDataAsync(TcpPackage tcpPackage)
        {
            if (sendingRightNow)
            {
                return;
            }
            sendingRightNow = true;
            await Task.Run(() => SendData(tcpPackage));
            sendingRightNow = false;
        }

        public void Dispose()
        {
            StopAcceptConnections();
            listener.Stop();
        }

        public void StopAcceptConnections()
        {
            listenForConnections = false;
        }
    }
}

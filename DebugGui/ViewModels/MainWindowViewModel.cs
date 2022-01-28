using DebugFramework.DataTypes.Responses;
using DebugFramework.Streaming;
using DebugFramework.Streaming.Clients.Broadcast;
using DebugFramework.Streaming.Clients.Communication;
using DebugFramework.Streaming.Package;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DebugGui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase WindowContent { get; private set; }

        public string Greeting => "Welcome to Avalonia!";

        public MainWindowViewModel()
        {
            UdpBroadcastServer<BroadcastData> broadcastClient = new UdpBroadcastServer<BroadcastData>(Configuration.BROADCAST_IP);
            List<int> ports = broadcastClient.GetFreePort(1024, 49150, 3);
            BroadcastData data = new BroadcastData()
            {
                ServerName = Environment.MachineName,
                IpAddress = broadcastClient.GetClientIp().ToString(),
                UpdatePort = ports[0],
                CommunicationRecievePort = ports[1],
                CommunicationSendPort = ports[2]
            };
            broadcastClient.StartBroadcast(new UdpPackage<BroadcastData>(), data);
            WindowContent = new MainDebugViewModel();

            Task.Run(async () =>
            {
                UdpCommunicationClient<BroadcastData> client = new UdpCommunicationClient<BroadcastData>(IPAddress.Any, data.CommunicationRecievePort, data.CommunicationSendPort);
                while (true)
                {
                    CommunicationPackage<BroadcastData> internalData = await client.RecieveCommunicationPackageAsync();
                    await Task.Delay(100);
                }

            });
        }
    }
}

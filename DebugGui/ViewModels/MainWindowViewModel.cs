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
            List<int> ports = broadcastClient.GetFreePort(1024, 49150, 2);
            BroadcastData data = new BroadcastData()
            {
                ServerName = Environment.MachineName,
                IpAddress = broadcastClient.GetClientIp().ToString(),
                UpdatePort = ports[0],
                CommunicationPort = ports[1]
            };
            broadcastClient.StartBroadcast(new UdpPackage<BroadcastData>(), data);
            WindowContent = new MainDebugViewModel();

            Task.Run(async () =>
            {
                UdpSendClient<BroadcastData> client = new UdpSendClient<BroadcastData>();
                UdpPackage<BroadcastData> testPackage = new UdpPackage<BroadcastData>();
                testPackage.Init(0, DataIdentifier.Login, new BroadcastData());
                while (true)
                {
                    client.SendTo(new IPEndPoint(IPAddress.Parse(data.IpAddress), data.UpdatePort), testPackage);
                    Task.Delay(500);
                }

            });

            Task.Run(async () =>
            {
                UdpRecieveClient<BroadcastData> client = new UdpRecieveClient<BroadcastData>(IPAddress.Parse(data.IpAddress), data.CommunicationPort);
                //UdpPackage<BroadcastData> testPackage = new UdpPackage<BroadcastData>();
                //testPackage.Init(0, DataIdentifier.Login, new BroadcastData());
                while (true)
                {
                    CommunicationPackage<BroadcastData> internalData = await client.RecieveCommunicationPackageAsync();
                    await Task.Delay(100);
                }

            });
        }
    }
}

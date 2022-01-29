using DebugFramework.DataTypes;
using DebugFramework.DataTypes.Responses;
using DebugFramework.DataTypes.SubTypes;
using DebugFramework.Streaming;
using DebugFramework.Streaming.Clients.Broadcast;
using DebugFramework.Streaming.Package;
using System;
using System.Collections.Generic;
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
            UdpPackage udpPackage = new UdpPackage();
            udpPackage.Init(0, DataIdentifier.Broadcast, data);
            broadcastClient.StartBroadcast(new UdpPackage(), data);
            WindowContent = new MainDebugViewModel();

            Task.Run(async () =>
            {
                UdpBroadcastServer<BaseDataType> updateClient = new UdpBroadcastServer<BaseDataType>(data.UpdatePort);
                UdpPackage broadcastPackage = new UdpPackage();
                uint packageNumber = 0;
                while (true)
                {
                    EntitesDump dump = new EntitesDump();
                    dump.Entites = new List<EntityContainer>() { new EntityContainer()
                    {
                        entityId = 0,
                        EntityComponents = new List<Component>()
                        {
                            new Component("Test")
                            {
                                Arguments = new List<ComponentArgument>()
                                {
                                    new ComponentArgument()
                                    {
                                        Name = "Texture",
                                        Value = "Some/texture/path"
                                    }
                                }
                            }
                        }
                    } };

                    broadcastPackage.Init(packageNumber, DataIdentifier.Update, dump);

                    updateClient.SendBroadcast(broadcastPackage);
                    packageNumber++;
                    await Task.Delay(16);
                }
            });
        }
    }
}


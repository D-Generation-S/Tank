using DebugFramework.DataTypes.Responses;
using DebugFramework.Streaming;
using DebugFramework.Streaming.Clients.Communication;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DebugGui.ViewModels
{
    public class MainDebugViewModel : ViewModelBase
    {
        private readonly UdpRecieveClient<BroadcastData> listener;

        private object gameInstanceLock = new object();

        public ObservableCollection<GameDebugInstanceViewModel> AvailableGameInstancs { get; }

        public GameDebugInstanceViewModel SelectedGameDebugInstance
        {
            get => selectedGameDebugInstance;
            set => this.RaiseAndSetIfChanged(ref selectedGameDebugInstance, value);
        }
        private GameDebugInstanceViewModel selectedGameDebugInstance;

        public bool IsConnected
        {
            get => isConnected;
            set => this.RaiseAndSetIfChanged(ref isConnected, value);
        }
        private bool isConnected;

        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnedtCommand { get; set; }

        public MainDebugViewModel()
        {
            listener = new UdpRecieveClient<BroadcastData>(Configuration.BROADCAST_IP);
            AvailableGameInstancs = new ObservableCollection<GameDebugInstanceViewModel>();
            IsConnected = false;

            Task.Run(async () =>
            {
                while (true)
                {
                    BroadcastData data = await listener.RecieveMessageAsync();
                    lock (gameInstanceLock)
                    {
                        if (AvailableGameInstancs.Any(instance => instance.IpAddress == data.IpAddress && instance.Port == data.UpdatePort))
                        {
                            continue;
                        }
                        AvailableGameInstancs.Add(new GameDebugInstanceViewModel(data));
                    }
                    _ = Task.Run(async () =>
                      {
                          UdpSendClient<BroadcastData> sendClient = new UdpSendClient<BroadcastData>();
                          while (true)
                          {
                              UdpRecieveClient<BroadcastData> internalTestListner = new UdpRecieveClient<BroadcastData>(IPAddress.Parse(data.IpAddress), data.UpdatePort);
                              CommunicationPackage<BroadcastData> internalData = await internalTestListner.RecieveCommunicationPackageAsync();
                              sendClient.SendTo(new IPEndPoint(internalData.Sender.Address, data.CommunicationPort), internalData.UdpPackage);
                              await Task.Delay(100);
                          }
                      });
                    await Task.Delay(100);
                }
            });



            IObservable<bool> canConnect = this.WhenAnyValue(
                                                    x => x.SelectedGameDebugInstance,
                                                    (GameDebugInstanceViewModel instance) => instance != null
                                                );

            ConnectCommand = ReactiveCommand.Create(async () =>
           {
               if (SelectedGameDebugInstance == null)
               {
                   return;
               }
               IsConnected = true;
               while (IsConnected)
               {
                   /**
                   await Task.Delay(16);
                   BaseDataType returnData = await listener.ListenForUpdatesAsync(SelectedGameDebugInstance.IpAddress, SelectedGameDebugInstance.Port, (package, type) =>
                   {
                       if (type == typeof(EntitesDump))
                       {
                           return package.GetPayload<EntitesDump>();
                       }
                       return null;
                   });
                   */
               }

           }, canConnect);
        }
    }
}

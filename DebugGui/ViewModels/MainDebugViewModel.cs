using DebugFramework.DataTypes;
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
        private readonly UdpRecieveClient listener;

        private object gameInstanceLock = new object();

        public ObservableCollection<GameDebugInstanceViewModel> AvailableGameInstancs { get; }

        public ObservableCollection<EntityViewModel> CurrentEntites { get; }

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

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        public MainDebugViewModel()
        {
            listener = new UdpRecieveClient(Configuration.BROADCAST_IP);
            AvailableGameInstancs = new ObservableCollection<GameDebugInstanceViewModel>();
            CurrentEntites = new ObservableCollection<EntityViewModel>();
            IsConnected = false;

            Task.Run(async () =>
            {
                while (true)
                {
                    BroadcastData data = await listener.RecieveMessageAsync<BroadcastData>();
                    lock (gameInstanceLock)
                    {
                        if (AvailableGameInstancs.Any(instance => instance.IpAddress == data.IpAddress && instance.Port == data.UpdatePort))
                        {
                            continue;
                        }
                        AvailableGameInstancs.Add(new GameDebugInstanceViewModel(data));
                    }
                    await Task.Delay(100);
                }
            });



            IObservable<bool> canConnect = this.WhenAnyValue(
                                                    x => x.SelectedGameDebugInstance,
                                                    (GameDebugInstanceViewModel instance) => instance != null
                                                );

            IObservable<bool> canDisconnect = this.WhenAnyValue(
                                        x => x.IsConnected,
                                        (bool connection) =>
                                        {
                                            return connection;
                                        });

            ConnectCommand = ReactiveCommand.Create(async () =>
            {
                if (SelectedGameDebugInstance == null)
                {
                    return;
                }
                IsConnected = true;
                UdpRecieveClient updateListner = new UdpRecieveClient(IPAddress.Parse(selectedGameDebugInstance.IpAddress), SelectedGameDebugInstance.Port);

                while (IsConnected)
                {
                    await Task.Delay(16);
                    CommunicationPackage returnData = await updateListner.RecieveCommunicationPackageAsync();
                    BaseDataType packageData = returnData.UdpPackage?.GetBasePayload();
                    if (packageData?.GetRealType() == typeof(EntitesDump))
                    {
                        EntitesDump dump = returnData.GetPackageContent<EntitesDump>();
                    }

                }

            }, canConnect);

            DisconnectCommand = ReactiveCommand.Create(async () =>
            {
                IsConnected = false;
            }, canDisconnect);
        }
    }
}

using Avalonia.Threading;
using DebugFramework.DataTypes;
using DebugFramework.DataTypes.Responses;
using DebugFramework.Streaming;
using DebugFramework.Streaming.Clients.Tcp;
using DebugFramework.Streaming.Clients.Udp.Communication;
using DebugFramework.Streaming.Package;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DebugGui.ViewModels
{
    public class MainDebugViewModel : ViewModelBase
    {
        private readonly UdpRecieveClient listener;

        private object gameInstanceLock = new object();

        public ObservableCollection<GameDebugInstanceViewModel> AvailableGameInstancs { get; }

        public ReadOnlyObservableCollection<EntityViewModel> CurrentEntities => currentEntities;

        private readonly ReadOnlyObservableCollection<EntityViewModel> currentEntities;

        private readonly SourceList<EntityViewModel> allCurrentEntities;

        public ViewModelBase SelectedEntityView
        {
            get => selectedEntityView;
            set => this.RaiseAndSetIfChanged(ref selectedEntityView, value);
        }
        private ViewModelBase selectedEntityView;

        public EntityViewModel SelectedEntity
        {
            get => selectedEntity;
            set => this.RaiseAndSetIfChanged(ref selectedEntity, value);
        }

        private EntityViewModel selectedEntity;

        public GameDebugInstanceViewModel SelectedGameDebugInstance
        {
            get => selectedGameDebugInstance;
            set => this.RaiseAndSetIfChanged(ref selectedGameDebugInstance, value);
        }
        private GameDebugInstanceViewModel selectedGameDebugInstance;

        private bool instancesPresent;

        public bool InstancesPresent
        {
            get => instancesPresent;
            set => this.RaiseAndSetIfChanged(ref instancesPresent, value);
        }

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
            allCurrentEntities = new();
            IsConnected = false;

            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(async data =>
            {
                DateTime currentTime = DateTime.Now;
                List<GameDebugInstanceViewModel> instancesToRemove = AvailableGameInstancs.Where(instance =>
                {
                    TimeSpan span = currentTime - instance.LastTimeFound;
                    return span.Seconds > 5;
                }).ToList();

                for (int i = instancesToRemove.Count; i > 0; i--)
                {
                    int index = i - 1;
                    if (instancesToRemove[index] == SelectedGameDebugInstance)
                    {
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            SelectedGameDebugInstance = null;
                            if (IsConnected)
                            {
                                DisconnectCommand?.Execute(null);
                            }
                        });

                    }
                    try
                    {
                        AvailableGameInstancs.RemoveAt(index);
                    }
                    catch (Exception)
                    {

                    }
                }
            });

            Task.Run(async () =>
            {
                while (true)
                {
                    BroadcastData data = await listener.RecieveMessageAsync<BroadcastData>();
                    lock (gameInstanceLock)
                    {
                        if (AvailableGameInstancs.Any(instance => instance.IpAddress == data.IpAddress && instance.Port == data.UpdatePort))
                        {
                            GameDebugInstanceViewModel foundInstance = AvailableGameInstancs.FirstOrDefault(instance => instance.IpAddress == data.IpAddress
                                                                                                            && instance.Port == data.UpdatePort
                                                                                                            && instance.MachineName == data.ServerName);
                            if (foundInstance != null)
                            {
                                foundInstance.LastTimeFound = DateTime.Now;
                            }
                            continue;
                        }
                        AvailableGameInstancs.Add(new GameDebugInstanceViewModel(data));
                    }
                    await Task.Delay(100);
                }
            });

            allCurrentEntities.Connect()
                              .Sort(SortExpressionComparer<EntityViewModel>.Ascending(g => g.EntityId))
                              .ObserveOn(AvaloniaScheduler.Instance)
                              .Bind(out currentEntities)
                              .Subscribe();


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
                TcpRecieverClient updateListner = new TcpRecieverClient(new IPEndPoint(IPAddress.Parse(selectedGameDebugInstance.IpAddress), SelectedGameDebugInstance.Port));
                //INetworkRecieveClient updateListner = new UdpRecieveClient(IPAddress.Parse(selectedGameDebugInstance.IpAddress), SelectedGameDebugInstance.Port);

                bool updateingRightNow = false;
                while (IsConnected)
                {
                    await Task.Delay(16);
                    TcpPackage dataPackage = await updateListner.RecieveDataPackageAsync();
                    if (dataPackage?.GetPayload().GetRealType() == typeof(EntitesDump))
                    {
                        EntitesDump dump = dataPackage.GetPayload<EntitesDump>();
                        if (!IsConnected)
                        {
                            updateListner.Dispose();
                            return;
                        }
                        if (updateingRightNow)
                        {
                            continue;
                        }
                        updateingRightNow = true;
                        List<EntityContainer> updatedEntites = dump.Entites;

                        IEnumerable<EntityContainer> newEntities = updatedEntites.Where(newEntity => !CurrentEntities.Any(cEntity => cEntity.EntityId == newEntity.EntityId));
                        IEnumerable<uint> removedEntityIds = CurrentEntities.Where(cEntity => !updatedEntites.Any(newEntity => newEntity.EntityId == cEntity.EntityId))
                                                                            .Select(container => container.EntityId);
                        IEnumerable<EntityViewModel> entitesToUpdate = CurrentEntities.Where(cEntity => updatedEntites.Any(newEntity => newEntity.EntityId == cEntity.EntityId));

                        for (int i = CurrentEntities.Count; i > 0; i--)
                        {
                            int index = i - 1;
                            if (removedEntityIds.Contains(CurrentEntities[index].EntityId))
                            {
                                allCurrentEntities.RemoveAt(index);
                            }
                        }

                        foreach (EntityContainer container in newEntities)
                        {
                            allCurrentEntities.Add(new EntityViewModel(container.EntityId, container.EntityComponents));
                        }

                        foreach (EntityViewModel container in entitesToUpdate)
                        {
                            EntityContainer updateBase = updatedEntites?.FirstOrDefault(uEntity => uEntity.EntityId == container.EntityId);
                            container?.UpdateComponents(updateBase?.EntityComponents);
                        }
                        this.RaisePropertyChanged(nameof(allCurrentEntities));
                        updateingRightNow = false;
                    }
                }
                updateListner.Dispose();

            }, canConnect);

            DisconnectCommand = ReactiveCommand.Create(async () =>
            {
                IsConnected = false;
                allCurrentEntities.Clear();
                SelectedEntity = null;
            }, canDisconnect);

            this.WhenAnyValue(x => x.SelectedEntity)
                .Subscribe(entity => SelectedEntityView = entity?.ComponentView);

            this.WhenAnyValue(x => x.AvailableGameInstancs.Count)
                .Subscribe(count => InstancesPresent = count > 0);
        }
    }
}

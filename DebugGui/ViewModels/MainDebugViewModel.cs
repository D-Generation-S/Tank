using Avalonia.Threading;
using DebugFramework.DataTypes;
using DebugFramework.DataTypes.Responses;
using DebugFramework.Streaming;
using DebugFramework.Streaming.Clients.Communication;
using DebugFramework.Streaming.Clients.Udp.Communication;
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
                IUdpRecieveClient updateListner = new UdpRecieveClient(IPAddress.Parse(selectedGameDebugInstance.IpAddress), SelectedGameDebugInstance.Port);

                while (IsConnected)
                {
                    await Task.Delay(16);
                    CommunicationPackage returnData = await updateListner.RecieveCommunicationPackageAsync();
                    BaseDataType packageData = returnData.UdpPackage?.GetPayload();
                    if (packageData?.GetRealType() == typeof(EntitesDump))
                    {
                        EntitesDump dump = returnData.GetPackageContent<EntitesDump>();
                        if (!IsConnected)
                        {
                            updateListner.Dispose();
                            return;
                        }

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
        }
    }
}

using DebugFramework.DataTypes;
using DebugFramework.DataTypes.Responses;
using DebugFramework.DataTypes.SubTypes;
using DebugFramework.Streaming;
using DebugFramework.Streaming.Clients.Udp.Broadcast;
using DebugFramework.Streaming.Package;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace TankEngine.EntityComponentSystem.Systems.Debugging
{
    public class UdpDebugSystem : AbstractSystem, IDisposable
    {
        private readonly Dictionary<Type, Func<IComponent, List<ComponentArgument>>> argumentConversion;

        private readonly UdpBroadcastServer<BroadcastData> broadcastClient;
        private readonly UdpBroadcastServer<BaseDataType> updateClient;
        private uint updatePackageNumber;

        private readonly UdpPackage updatePackage;
        private readonly EntitesDump entitesDump;

        public UdpDebugSystem(Dictionary<Type, Func<IComponent, List<ComponentArgument>>> customComponentConversions)
        {
            argumentConversion = customComponentConversions ?? new Dictionary<Type, Func<IComponent, List<ComponentArgument>>>();

            broadcastClient = new UdpBroadcastServer<BroadcastData>(Configuration.BROADCAST_IP);
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

            updatePackageNumber = 0;
            updateClient = new UdpBroadcastServer<BaseDataType>(data.UpdatePort);

            updatePackage = new UdpPackage();
            entitesDump = new EntitesDump();

            argumentConversion.Add(typeof(PositionComponent), (component) =>
            {
                List<ComponentArgument> arguments = new List<ComponentArgument>();
                if (component is PositionComponent position)
                {
                    arguments.Add(new ComponentArgument("Rotation", position.Rotation.ToString()));
                    arguments.Add(new ComponentArgument("Position", string.Format("X: {0}, Y {1}", position.Position.X, position.Position.Y)));
                }
                return arguments;
            });

            argumentConversion.Add(typeof(TextComponent), (component) =>
            {
                List<ComponentArgument> arguments = new List<ComponentArgument>();
                return arguments;
                if (component is TextComponent textComponent)
                {
                    arguments.AddRange(GetRenderingComponentBase(textComponent));
                    arguments.Add(new ComponentArgument("Text", textComponent.Text));
                    arguments.Add(new ComponentArgument("Font", textComponent.Font.Texture.Name));
                    arguments.Add(new ComponentArgument("Scale", textComponent.Scale.ToString()));
                }
                return arguments;
            });

            argumentConversion.Add(typeof(TextureComponent), (component) =>
            {
                List<ComponentArgument> arguments = new List<ComponentArgument>();
                return arguments;
                if (component is TextureComponent textureComponent)
                {
                    arguments.AddRange(GetRenderingComponentBase(textureComponent));
                    //arguments.Add(new ComponentArgument("Rotation", textureComponent.Rotation.ToString()));
                    //arguments.Add(new ComponentArgument("Position", string.Format("X: {0}, Y {1}", textureComponent.Position.X, textureComponent.Position.Y)));
                }
                return arguments;
            });

        }

        private List<ComponentArgument> GetRenderingComponentBase(AbstractRenderingComponent renderingComponent)
        {
            List<ComponentArgument> arguments = new List<ComponentArgument>();
            arguments.Add(new ComponentArgument("Visible", renderingComponent.Visible.ToString()));
            arguments.Add(new ComponentArgument("Color", renderingComponent.Color.ToString()));
            arguments.Add(new ComponentArgument("SpriteEffect", renderingComponent.SpriteEffect.ToString()));
            arguments.Add(new ComponentArgument("ShaderEffect", renderingComponent.ShaderEffect?.Name));
            arguments.Add(new ComponentArgument("DrawLayer", renderingComponent.DrawLayer.ToString()));
            arguments.Add(new ComponentArgument("RotationCenter", renderingComponent.RotationCenter.ToString()));
            arguments.Add(new ComponentArgument("DrawOffset", renderingComponent.DrawOffset.ToString()));
            return arguments;
        }

        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
            validators.Add(new AllEntitesValidator());
        }

        public override void Update(GameTime gameTime)
        {
            entitesDump.Entites.Clear();

            foreach (uint entityId in watchedEntities)
            {
                List<IComponent> entityComponents = entityManager.GetComponents(entityId);
                List<ComponentData> components = entityComponents.Select(component =>
                {
                    ComponentData componentData = new ComponentData(component.GetType().Name);
                    if (argumentConversion.ContainsKey(component.GetType()))
                    {
                        componentData.Arguments = argumentConversion[component.GetType()](component);
                    }

                    return componentData;
                }).ToList();
                entitesDump.Entites.Add(new EntityContainer()
                {
                    EntityId = entityId,
                    EntityComponents = components
                });
            }
            updatePackage.Init(updatePackageNumber, DataIdentifier.Update, entitesDump);
            updateClient.SendBroadcast(updatePackage);
        }
        public void Dispose()
        {
            broadcastClient?.Dispose();
            updateClient?.Dispose();
        }
    }
}

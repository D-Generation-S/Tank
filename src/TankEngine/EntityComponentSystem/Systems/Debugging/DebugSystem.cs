using DebugFramework;
using DebugFramework.DataTypes;
using DebugFramework.DataTypes.Responses;
using DebugFramework.DataTypes.SubTypes;
using DebugFramework.Streaming;
using DebugFramework.Streaming.Clients.Tcp;
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
    public class DebugSystem : AbstractSystem, IDisposable
    {
        private readonly StreamingDataTypeManager streamingDataTypeManager;
        private readonly Dictionary<Type, Func<IComponent, StreamingDataTypeManager, List<ComponentArgument>>> argumentConversion;

        private readonly UdpBroadcastServer<BroadcastData> broadcastClient;
        private readonly TcpSendOnlyServer updateClient;

        private readonly TcpPackage updatePackage;
        private readonly EntitesDump entitesDump;

        public DebugSystem(Dictionary<Type, Func<IComponent, StreamingDataTypeManager, List<ComponentArgument>>> customComponentConversions)
        {
            streamingDataTypeManager = new StreamingDataTypeManager();
            argumentConversion = customComponentConversions ?? new Dictionary<Type, Func<IComponent, StreamingDataTypeManager, List<ComponentArgument>>>();

            broadcastClient = new UdpBroadcastServer<BroadcastData>(Configuration.BROADCAST_PORT);
            List<int> ports = broadcastClient.GetFreePort(1024, 49150, 3);
            BroadcastData data = new BroadcastData()
            {
                ServerName = Environment.MachineName,
                IpAddress = broadcastClient.GetLocalIp().ToString(),
                UpdatePort = ports[0],
                CommunicationRecievePort = ports[1],
                CommunicationSendPort = ports[2]
            };
            UdpPackage udpPackage = new UdpPackage();
            udpPackage.Init(0, DataIdentifier.Broadcast, data);
            broadcastClient.StartBroadcastAsync(new UdpPackage(), data);

            updateClient = new TcpSendOnlyServer(data.UpdatePort);
            updateClient.AcceptConnectionsAsync();

            updatePackage = new TcpPackage();
            entitesDump = new EntitesDump();

            argumentConversion.Add(typeof(PositionComponent), (component, dataTypeManager) =>
            {
                List<ComponentArgument> arguments = new List<ComponentArgument>();
                if (component is PositionComponent position)
                {
                    ComponentArgument rotationArg = dataTypeManager.GetDataType<ComponentArgument>();
                    rotationArg.Name = "Rotation";
                    rotationArg.Value = position.Rotation.ToString();
                    arguments.Add(rotationArg);

                    ComponentArgument positionArg = dataTypeManager.GetDataType<ComponentArgument>();
                    positionArg.Name = "Position";
                    positionArg.Value = position.Position.ToString();
                    arguments.Add(positionArg);
                }
                return arguments;
            });

            argumentConversion.Add(typeof(TextComponent), (component, dataTypeManager) =>
            {
                List<ComponentArgument> arguments = new List<ComponentArgument>();
                if (component is TextComponent textComponent)
                {
                    arguments.AddRange(GetRenderingComponentBase(textComponent, dataTypeManager));
                    ComponentArgument text = dataTypeManager.GetDataType<ComponentArgument>();
                    text.Name = "Text";
                    text.Value = textComponent.Text;
                    ComponentArgument font = dataTypeManager.GetDataType<ComponentArgument>();
                    font.Name = "Font";
                    font.Value = textComponent.Font.Texture.Name;
                    ComponentArgument scale = dataTypeManager.GetDataType<ComponentArgument>();
                    scale.Name = "Scale";
                    scale.Value = textComponent.Scale.ToString();
                    arguments.Add(text);
                    arguments.Add(font);
                    arguments.Add(scale);
                }
                return arguments;
            });

            argumentConversion.Add(typeof(TextureComponent), (component, dataTypeManager) =>
            {
                List<ComponentArgument> arguments = new List<ComponentArgument>();
                if (component is TextureComponent textureComponent)
                {
                    arguments.AddRange(GetRenderingComponentBase(textureComponent, dataTypeManager));
                    ComponentArgument texture = dataTypeManager.GetDataType<ComponentArgument>();
                    texture.Name = "Texture";
                    texture.Value = textureComponent?.Texture?.Name;
                    ComponentArgument source = dataTypeManager.GetDataType<ComponentArgument>();
                    source.Name = "Source";
                    source.Value = textureComponent.Source.ToString();
                    ComponentArgument scale = dataTypeManager.GetDataType<ComponentArgument>();
                    scale.Name = "Scale";
                    scale.Value = textureComponent.Scale.ToString();
                    arguments.Add(texture);
                    arguments.Add(source);
                    arguments.Add(scale);
                }
                return arguments;
            });
        }

        private List<ComponentArgument> GetRenderingComponentBase(AbstractRenderingComponent renderingComponent, StreamingDataTypeManager streamingDataTypeManager)
        {
            List<ComponentArgument> arguments = new List<ComponentArgument>();
            ComponentArgument visible = streamingDataTypeManager.GetDataType<ComponentArgument>();
            visible.Name = "Visible";
            visible.Value = renderingComponent.Visible.ToString();
            ComponentArgument color = streamingDataTypeManager.GetDataType<ComponentArgument>();
            color.Name = "Color";
            color.Value = renderingComponent.Color.ToString();
            ComponentArgument spriteEffect = streamingDataTypeManager.GetDataType<ComponentArgument>();
            spriteEffect.Name = "Sprite Effect";
            spriteEffect.Value = renderingComponent.SpriteEffect.ToString();
            ComponentArgument shaderEffect = streamingDataTypeManager.GetDataType<ComponentArgument>();
            shaderEffect.Name = "Shader Effect";
            shaderEffect.Value = renderingComponent.ShaderEffect?.Name;
            ComponentArgument drawLayer = streamingDataTypeManager.GetDataType<ComponentArgument>();
            drawLayer.Name = "Draw Layer";
            drawLayer.Value = renderingComponent.DrawLayer.ToString();
            ComponentArgument rotationCenter = streamingDataTypeManager.GetDataType<ComponentArgument>();
            rotationCenter.Name = "Rotation Center";
            rotationCenter.Value = renderingComponent.RotationCenter.ToString();
            ComponentArgument drawOffset = streamingDataTypeManager.GetDataType<ComponentArgument>();
            drawOffset.Name = "Draw Offset";
            drawOffset.Value = renderingComponent.DrawOffset.ToString();
            arguments.Add(visible);
            arguments.Add(color);
            arguments.Add(spriteEffect);
            arguments.Add(shaderEffect);
            arguments.Add(drawLayer);
            arguments.Add(rotationCenter);
            arguments.Add(drawOffset);
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
                    streamingDataTypeManager.GetDataType<ComponentData>();
                    ComponentData componentData = streamingDataTypeManager.GetDataType<ComponentData>();
                    componentData.ComponentType = component.GetType().Name;
                    if (argumentConversion.ContainsKey(component.GetType()))
                    {
                        componentData.Arguments = argumentConversion[component.GetType()](component, streamingDataTypeManager);
                    }

                    return componentData;
                }).ToList();
                EntityContainer entityContainer = streamingDataTypeManager.GetDataType<EntityContainer>();
                entityContainer.EntityId = entityId;
                entityContainer.EntityComponents = components;
                entitesDump.Entites.Add(entityContainer);
            }
            updatePackage.Init(DataIdentifier.Update, entitesDump);
            updateClient.SendDataAsync(updatePackage);

            foreach (EntityContainer container in entitesDump.Entites)
            {
                foreach (ComponentData componentData in container.EntityComponents)
                {
                    foreach (ComponentArgument argument in componentData.Arguments)
                    {
                        streamingDataTypeManager.ReturnBaseData(argument);
                    }
                    componentData.Arguments.Clear();
                    streamingDataTypeManager.ReturnBaseData(componentData);
                }
                container.EntityComponents.Clear();
                streamingDataTypeManager.ReturnBaseData(container);
            }
        }
        public void Dispose()
        {
            broadcastClient?.Dispose();
            updateClient?.Dispose();
        }
    }
}

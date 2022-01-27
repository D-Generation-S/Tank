using DebugFramework;
using DebugFramework.DataTypes;
using DebugFramework.DataTypes.Requests;
using DebugFramework.DataTypes.SubTypes;
using DebugFramework.Streaming;
using DebugFramework.Streaming.Conversion;
using Microsoft.Xna.Framework;
using System.IO.Pipes;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace TankEngine.EntityComponentSystem.Systems.Debugging
{
    public class PipeDebugSystem : AbstractSystem
    {
        private PipeStreamServer pipeStreamServer;
        private StreamingDataTypeManager typeManager;

        public PipeDebugSystem()
        {
            pipeStreamServer = new PipeStreamServer();
            typeManager = new StreamingDataTypeManager();
            pipeStreamServer.StartListenAsync();
            pipeStreamServer.AddResponseAction(typeof(EntityDumpRequest), new LambdaResponseAction<NamedPipeServerStream>((type, data, stream, communicator) =>
            {
                EntitesDump dump = typeManager.GetPipeBaseData<EntitesDump>();
                foreach (uint entityId in watchedEntities)
                {
                    EntityContainer container = typeManager.GetPipeBaseData<EntityContainer>();
                    container.entityId = entityId;
                    foreach (IComponent component in entityManager.GetComponents(entityId))
                    {
                        Component dumpComponent = typeManager.GetPipeBaseData<Component>();
                        dumpComponent.ComponentType = component.GetType().FullName;
                    }
                }
                communicator.WriteToStream(stream, dump);
                foreach (EntityContainer container in dump.Entites)
                {
                    foreach (Component component in container.EntityComponents)
                    {
                        foreach (ComponentArgument argument in component.Arguments)
                        {
                            argument.Name = string.Empty;
                            argument.Value = string.Empty;
                            typeManager.ReturnBaseData(argument);
                        }
                        component.ComponentType = string.Empty;
                        component.Arguments.Clear();
                        typeManager.ReturnBaseData(component);
                    }

                    container.EntityComponents.Clear();
                    typeManager.ReturnBaseData(container);
                }
                return true;
            }));
        }

        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
            validators.Add(new AllEntitesValidator());
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}

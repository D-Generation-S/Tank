using DebugFramework;
using Microsoft.Xna.Framework;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace TankEngine.EntityComponentSystem.Systems.Debugging
{
    public class PipeDebugSystem : AbstractSystem
    {
        private StreamingDataTypeManager typeManager;

        public PipeDebugSystem()
        {

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

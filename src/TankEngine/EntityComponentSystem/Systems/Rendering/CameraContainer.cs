using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;

namespace TankEngine.EntityComponentSystem.Systems.Rendering
{
    /// <summary>
    /// Container for a camera entity
    /// </summary>
    public class CameraContainer
    {
        public PositionComponent Position { get; set; }
        public CameraComponent Camera { get; set; }

        public CameraContainer()
        {
            Position = null;
            Camera = null;
        }
    }
}

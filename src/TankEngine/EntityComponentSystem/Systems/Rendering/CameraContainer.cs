using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;

namespace TankEngine.EntityComponentSystem.Systems.Rendering
{
    /// <summary>
    /// Container for a camera entity
    /// </summary>
    public class CameraContainer
    {
        /// <summary>
        /// The position of the camera in the world
        /// </summary>
        public PositionComponent WorldPosition { get; set; }

        /// <summary>
        /// The camera element with information for rendering
        /// </summary>
        public CameraComponent Camera { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public CameraContainer()
        {
            WorldPosition = null;
            Camera = null;
        }
    }
}

using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;

namespace TankEngine.EntityComponentSystem.Systems.Rendering
{
    /// <summary>
    /// Container for texture rendering
    /// </summary>
    public class TextureRenderContainer
    {
        /// <summary>
        /// The position component used for getting the position
        /// </summary>
        public PositionComponent PositionComponent { get; set; }

        /// <summary>
        /// The texture component used for drawing
        /// </summary>
        public TextureComponent TextureComponent { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public TextureRenderContainer()
        {
            PositionComponent = null;
            TextureComponent = null;
        }
    }
}

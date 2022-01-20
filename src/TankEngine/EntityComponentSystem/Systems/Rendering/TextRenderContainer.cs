using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;

namespace TankEngine.EntityComponentSystem.Systems.Rendering
{
    public class TextRenderContainer
    {
        /// <summary>
        /// The position component used for getting the position
        /// </summary>
        public PositionComponent PositionComponent { get; set; }

        /// <summary>
        /// The text component which should be rendered
        /// </summary>
        public TextComponent TextComponent { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public TextRenderContainer()
        {
            PositionComponent = null;
            TextComponent = null;
        }
    }
}

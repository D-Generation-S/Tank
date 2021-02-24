using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tank.Components.Rendering
{
    /// <summary>
    /// Base class for rendering system
    /// </summary>
    internal abstract class BaseVisibleComponent : BaseComponent
    {
        /// <summary>
        /// The draw color for the visible
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The effect to use
        /// </summary>
        public SpriteEffects Effect;

        /// <summary>
        /// The depth of the layer
        /// </summary>
        public float LayerDepth;

        /// <inheritdoc/>
        public override void Init()
        {
            Color = Color.White;
            LayerDepth = 1f;
            Effect = SpriteEffects.None;
        }
    }
}

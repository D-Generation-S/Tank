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
        public Color Color;

        /// <summary>
        /// The effect to use
        /// </summary>
        public SpriteEffects Effect;

        /// <summary>
        /// The shader effect to use
        /// </summary>
        public Effect ShaderEffect;

        /// <summary>
        /// The depth of the layer
        /// </summary>
        public float LayerDepth;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public BaseVisibleComponent()
        {
            Priority = 500;
        }

        /// <inheritdoc/>
        public override void Init()
        {
            Color = Color.White;
            LayerDepth = 1f;
            Effect = SpriteEffects.None;
            ShaderEffect = null;
        }
    }
}

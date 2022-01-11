using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TankEngine.EntityComponentSystem.Components;

namespace Tank.Components.Rendering
{
    /// <summary>
    /// Base class for rendering system
    /// </summary>
    internal abstract class BaseVisibleComponent : BaseComponent
    {
        /// <summary>
        /// Is this texture hidden right now
        /// </summary>
        public bool Hidden;

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
        /// The center of rotiation
        /// </summary>
        public Vector2 RotationCenter;

        /// <inheritdoc/>
        public override void Init()
        {
            Hidden = false;
            Color = Color.White;
            Effect = SpriteEffects.None;
            ShaderEffect = null;
            LayerDepth = 0f;
            RotationCenter = Vector2.Zero;
        }
    }
}

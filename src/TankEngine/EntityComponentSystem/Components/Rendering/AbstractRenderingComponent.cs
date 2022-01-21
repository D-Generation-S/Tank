using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankEngine.EntityComponentSystem.Components.Rendering
{
    public class AbstractRenderingComponent : BaseComponent
    {
        /// <summary>
        /// Should the entity be rendered right now
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// The draw color of the visible
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The sprite effect to use for rendering
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        /// A shader effect to use for the given render component
        /// </summary>
        public Effect ShaderEffect { get; set; }

        /// <summary>
        /// The layer to draw the sprite on must be between 0 and 1000
        /// </summary>
        public int DrawLayer { get; set; }

        /// <summary>
        /// The point to use on the sprite/text to roatet around
        /// </summary>
        public Vector2 RotationCenter { get; set; }

        /// <summary>
        /// The offset to use for drawing relative to the entity position
        /// </summary>
        public Vector2 DrawOffset { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            Visible = true;
            Color = Color.White;
            SpriteEffect = SpriteEffects.None;
            DrawLayer = 0;
            RotationCenter = Vector2.Zero;
            DrawOffset = Vector2.Zero;
        }
    }
}

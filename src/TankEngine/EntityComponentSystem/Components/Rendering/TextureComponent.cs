using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankEngine.EntityComponentSystem.Components.Rendering
{
    /// <summary>
    /// Texture component to draw
    /// </summary>
    public class TextureComponent : AbstractRenderingComponent
    {
        /// <summary>
        /// The texture to draw
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The rectangle to use for getting the correct part of the texture
        /// </summary>
        public Rectangle Source { get; set; }

        /// <summary>
        /// The offset to use for drawing relative to the entity position
        /// </summary>
        public Vector2 DrawOffset { get; set; }

        /// <summary>
        /// The scale of the image to use
        /// </summary>
        public float Scale;

        /// <inheritdoc/>
        public override void Init()
        {
            base.Init();
            Texture = null;
            Source = Rectangle.Empty;
            DrawOffset = Vector2.Zero;
            Scale = 1.0f;
        }
    }
}

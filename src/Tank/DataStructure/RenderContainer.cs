using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tank.DataStructure
{
    /// <summary>
    /// Container to used for rendering
    /// </summary>
    public class RenderContainer
    {
        /// <summary>
        /// The texture to draw
        /// </summary>
        public Texture2D TextureToDraw;

        /// <summary>
        /// The destination of the texture
        /// </summary>
        public Rectangle Destination;

        /// <summary>
        /// The source of the texture
        /// </summary>
        public Rectangle Source;

        /// <summary>
        /// The color of the texture to use
        /// </summary>
        public Color Color;

        /// <summary>
        /// The rotation of the texture
        /// </summary>
        public float Rotation;

        /// <summary>
        /// The rotaion axis center
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// The effect to apply 
        /// </summary>
        public SpriteEffects Effect;

        /// <summary>
        /// The layer this is drawn on
        /// </summary>
        public float LayerDepth;

        /// <summary>
        /// Create a new instance of this object
        /// </summary>
        public RenderContainer()
        {
            Origin = Vector2.Zero;
            Effect = SpriteEffects.None;
            LayerDepth = 0;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TankEngine.Enums;

namespace TankEngine.DataStructures
{
    /// <summary>
    /// Container to used for rendering
    /// </summary>
    public class RenderContainer
    {
        /// <summary>
        /// The name of the container
        /// </summary>
        public string Name;

        /// <summary>
        /// The render type for this container
        /// </summary>
        public RenderTypeEnum RenderType;

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
        /// The shader effect to use
        /// </summary>
        public Effect ShaderEffect;

        /// <summary>
        /// The layer this is drawn on
        /// </summary>
        public float LayerDepth;

        /// <summary>
        /// The position to render the text
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The text to write
        /// </summary>
        public string Text;

        /// <summary>
        /// The font to use
        /// </summary>
        public SpriteFont Font;

        /// <summary>
        /// The scale of the text
        /// </summary>
        public float Scale;

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

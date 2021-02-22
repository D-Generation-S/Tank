using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tank.Components
{
    /// <summary>
    /// This component will make a component visible in the gameworld
    /// </summary>
    class VisibleComponent : BaseComponent
    {
        /// <summary>
        /// The texture or sprite to use for the entity
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Public access to the texture
        /// </summary>
        public Texture2D Texture
        {
            get => texture;
            set => texture = value;
        }

        /// <summary>
        /// Where should the sprite be drawn to
        /// </summary>
        private Rectangle destination;

        /// <summary>
        /// public access to the draw position
        /// </summary>
        public Rectangle Destination
        {
            get => destination;
            set => destination = value;
        }

        /// <summary>
        /// The source position of the part in the sprite which should be drawn
        /// </summary>
        private Rectangle source;

        /// <summary>
        /// Public access to the draw position in the sprite
        /// </summary>
        public Rectangle Source
        {
            get => source;
            set => source = value;
        }

        /// <summary>
        /// The color to draw the texture with
        /// </summary>
        private Color color;

        /// <summary>
        /// Public access to the draw color for the texture
        /// </summary>
        public Color Color
        {
            get => color;
            set => color = value;
        }

        /// <summary>
        /// Create a new instance of this component
        /// </summary>
        public VisibleComponent() : this(Color.White)
        {
        }

        /// <summary>
        /// Create a new instance of this component
        /// </summary>
        public VisibleComponent(Color color) : this(color, null)
        {
        }

        /// <summary>
        /// Create a new instance of this component
        /// </summary>
        public VisibleComponent(Color color, Texture2D texture)
        {
            this.color = color;
            this.texture = texture;
            if (texture != null)
            {
                destination = new Rectangle(0, 0, texture.Width, texture.Height);
                source = destination;
            }

        }
    }
}

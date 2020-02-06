using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.Implementations;

namespace Tank.Code.Implementations
{
    class SpriteRenderer : IRenderer
    {
        protected Texture2D texture;
        public Texture2D Texture {
            get => texture;
            set
            {
                if (!textureLocked)
                {
                    texture = value;
                    BuildSourceRectangle();
                    textureLocked = true;
                }
            }
        }

        public bool IsReady => texture != null;

        private bool textureLocked;
        public bool TextureLocked => textureLocked;

        private Vector2 size;
        public Vector2 Size {
            get => size;
            set
            {
                size = value;
                if (position != null)
                {
                    BuildDestionationRectangle();
                }
            }
        }

        private Vector2 position;
        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                if (size != null)
                {
                    BuildDestionationRectangle();
                }
            }
        }

        protected Rectangle destination;
        public Rectangle Destination => destination;

        protected Rectangle source;
        public Rectangle Source => source;
            

        public virtual void DrawStep()
        {
            
        }

        protected virtual void BuildSourceRectangle()
        {
            source = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        protected virtual void BuildDestionationRectangle()
        {
            destination = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public virtual void Reset()
        {
            textureLocked = false;
            texture = null;
        }
    }
}

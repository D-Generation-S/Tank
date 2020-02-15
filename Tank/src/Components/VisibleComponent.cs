using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Components
{
    class VisibleComponent : BaseComponent
    {
        private Texture2D texture;
        public Texture2D Texture
        {
            get => texture;
            set => texture = value;
        }

        private Rectangle destination;
        public Rectangle Destination
        {
            get => destination;
            set => destination = value;
        }

        private Rectangle source;
        public Rectangle Source
        {
            get => source;
            set => source = value;
        }

        private Color color;
        public Color Color
        {
            get => color;
            set => color = value;
        }

        public VisibleComponent()
        {
            color = Color.White;
        }
    }
}

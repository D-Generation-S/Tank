using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.DataStructure
{
    public class RenderContainer
    {
        public Texture2D TextureToDraw;
        public Rectangle Destination;
        public Rectangle Source;
        public Color Color;
        public float Rotation;
        public Vector2 Origin;
        public SpriteEffects Effect;
        public float LayerDepth;

        public RenderContainer()
        {
            Origin = Vector2.Zero;
            Effect = SpriteEffects.None;
            LayerDepth = 0;
        }
    }
}

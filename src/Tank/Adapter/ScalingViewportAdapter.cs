using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Adapter
{
    class ScalingViewportAdapter : ViewportAdapter
    {
        public override int VirtualWidth { get; }
        public override int VirtualHeight { get; }
        public override int ViewportWidth => Viewport.Width;
        public override int ViewportHeight => Viewport.Height;

        public override Rectangle BoundingRectangle { get; }

        public override Viewport VirtualViewport { get; }

        public ScalingViewportAdapter(GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight) : base(graphicsDevice)
        {
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            BoundingRectangle = new Rectangle(0, 0, virtualWidth, virtualHeight);
            VirtualViewport = new Viewport(BoundingRectangle);
        }

        public override Matrix GetScaleMatrix()
        {
            float scaleX = ViewportWidth / (float)VirtualWidth;
            float scaleY = ViewportHeight / (float)VirtualHeight;
            return Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }
    }
}

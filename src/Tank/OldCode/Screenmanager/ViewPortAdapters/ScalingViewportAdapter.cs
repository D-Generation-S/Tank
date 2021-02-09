﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tank.Code.Screenmanager.ViewPortAdapters
{
    public class ScalingViewportAdapter : ViewPortAdapter
    {
        public ScalingViewportAdapter(GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight)
            : base(graphicsDevice)
        {
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
        }

        public override int VirtualWidth
        {
            get;
        }
        public override int VirtualHeight
        {
            get;
        }
        public override int ViewportWidth => GraphicsDevice.Viewport.Width;
        public override int ViewportHeight => GraphicsDevice.Viewport.Height;

        public override Matrix GetScaleMatrix()
        {
            var scaleX = (float)ViewportWidth / VirtualWidth;
            var scaleY = (float)ViewportHeight / VirtualHeight;
            return Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }
    }
}

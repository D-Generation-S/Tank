using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Adapter
{
    /// <summary>
    /// A viewport which will strecht the image to match the resolution
    /// </summary>
    class ScalingViewportAdapter : ViewportAdapter
    {
        /// <inheritdoc/>
        public override int VirtualWidth { get; }

        /// <inheritdoc/>
        public override int VirtualHeight { get; }

        /// <inheritdoc/>
        public override int ViewportWidth => Viewport.Width;

        /// <inheritdoc/>
        public override int ViewportHeight => Viewport.Height;

        /// <inheritdoc/>
        public override Rectangle BoundingRectangle { get; }

        /// <inheritdoc/>
        public override Viewport VirtualViewport { get; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="graphicsDevice">The graphics device to use</param>
        /// <param name="virtualWidth">The virtual width</param>
        /// <param name="virtualHeight">The virtual height</param>
        public ScalingViewportAdapter(GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight) : base(graphicsDevice)
        {
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            BoundingRectangle = new Rectangle(0, 0, virtualWidth, virtualHeight);
            VirtualViewport = new Viewport(BoundingRectangle);
        }

        /// <inheritdoc/>
        public override Matrix GetScaleMatrix()
        {
            float scaleX = ViewportWidth / (float)VirtualWidth;
            float scaleY = ViewportHeight / (float)VirtualHeight;
            return Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Adapter
{
    abstract class ViewportAdapter : IViewportAdapter
    {
        public abstract int VirtualWidth { get; }
        public abstract int VirtualHeight { get; }
        public abstract int ViewportWidth { get; }
        public abstract int ViewportHeight { get; }

        public Point Center => BoundingRectangle.Center;
        public abstract Rectangle BoundingRectangle { get; }

        public Viewport Viewport => graphicsDevice.Viewport;
        public abstract Viewport VirtualViewport { get; }

        protected GraphicsDevice graphicsDevice;

        public ViewportAdapter(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public abstract Matrix GetScaleMatrix();

        public Point GetPointOnScreen(Point point)
        {
            return GetPointOnScreen(point.ToVector2());
        }

        public Point GetPointOnScreen(Vector2 point)
        {
            return GetVectorPointOnScreen(point).ToPoint();
        }

        public Vector2 GetVectorPointOnScreen(Point point)
        {
            return GetVectorPointOnScreen(point.ToVector2());
        }

        public virtual Vector2 GetVectorPointOnScreen(Vector2 point)
        {
            Matrix scaleMatrix = GetScaleMatrix();
            scaleMatrix = Matrix.Invert(scaleMatrix);
            return Vector2.Transform(point, scaleMatrix);
        }

        public virtual void Reset()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}

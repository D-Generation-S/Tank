using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankEngine.Adapter
{
    /// <summary>
    /// Abstract viewport adapter
    /// </summary>
    public abstract class ViewportAdapter : IViewportAdapter
    {
        /// <inheritdoc/>
        public abstract int VirtualWidth { get; }

        /// <inheritdoc/>
        public abstract int VirtualHeight { get; }

        /// <inheritdoc/>
        public abstract int ViewportWidth { get; }

        /// <inheritdoc/>
        public abstract int ViewportHeight { get; }

        /// <inheritdoc/>
        public Point Center => BoundingRectangle.Center;

        /// <inheritdoc/>
        public abstract Rectangle BoundingRectangle { get; }

        /// <inheritdoc/>
        public Viewport Viewport => graphicsDevice.Viewport;

        /// <inheritdoc/>
        public abstract Viewport VirtualViewport { get; }

        /// <summary>
        /// The graphics device to use
        /// </summary>
        protected GraphicsDevice graphicsDevice;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="graphicsDevice">The grahpics device to use</param>
        public ViewportAdapter(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        /// <inheritdoc/>
        public abstract Matrix GetScaleMatrix();

        /// <inheritdoc/>
        public Point GetPointOnScreen(Point point)
        {
            return GetPointOnScreen(point.ToVector2());
        }

        /// <inheritdoc/>
        public Point GetPointOnScreen(Vector2 point)
        {
            return GetVectorPointOnScreen(point).ToPoint();
        }

        /// <inheritdoc/>
        public Vector2 GetVectorPointOnScreen(Point point)
        {
            return GetVectorPointOnScreen(point.ToVector2());
        }

        /// <inheritdoc/>
        public virtual Vector2 GetVectorPointOnScreen(Vector2 point)
        {
            Matrix scaleMatrix = GetScaleMatrix();
            scaleMatrix = Matrix.Invert(scaleMatrix);
            return Vector2.Transform(point, scaleMatrix);
        }

        /// <inheritdoc/>
        public virtual void Reset()
        {
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tank.Adapter
{
    /// <summary>
    /// Adapter to create a box format
    /// </summary>
    class BoxingViewportAdapter : ScalingViewportAdapter
    {
        /// <summary>
        /// The window the game is running in
        /// </summary>
        private readonly GameWindow window;

        /// <summary>
        /// The horizontal bleed
        /// </summary>
        private readonly int horizontalBleed;

        /// <summary>
        /// The vertical bleed
        /// </summary>
        private readonly int verticalBleed;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="window">The window the game is running in</param>
        /// <param name="graphicsDevice">The current graphic device</param>
        /// <param name="virtualWidth">The virtual width to use</param>
        /// <param name="virtualHeight">The virtual height to use</param>
        public BoxingViewportAdapter(GameWindow window, GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight) : this(window, graphicsDevice, virtualWidth, virtualHeight, 0, 0)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="window">The window the game is running in</param>
        /// <param name="graphicsDevice">The current graphic device</param>
        /// <param name="virtualWidth">The virtual width to use</param>
        /// <param name="virtualHeight">The virtual height to use</param>
        /// <param name="horizontalBleed">The horizontal bleed</param>
        /// <param name="verticalBleed">The vertical bleed</param>
        public BoxingViewportAdapter(GameWindow window, GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight, int horizontalBleed, int verticalBleed) : base(graphicsDevice, virtualWidth, virtualHeight)
        {
            this.window = window;
            window.ClientSizeChanged += (sender, data) => OnClienteSizeChanged();
            this.horizontalBleed = horizontalBleed;
            this.verticalBleed = verticalBleed;
            RecalculateBox();
        }

        /// <summary>
        /// Event if client size is getting changed
        /// </summary>
        public void OnClienteSizeChanged()
        {
            RecalculateBox();
        }


        /// <summary>
        /// Recalculate the box
        /// </summary>
        private void RecalculateBox()
        {
            Rectangle clientBounds = window.ClientBounds;

            float worldScaleX = (float)clientBounds.Width / VirtualWidth;
            float worldScaleY = (float)clientBounds.Height / VirtualHeight;

            float safeScaleX = (float)clientBounds.Width / (VirtualWidth - horizontalBleed);
            float safeScaleY = (float)clientBounds.Height / (VirtualHeight - verticalBleed);

            float worldScale = MathHelper.Max(worldScaleX, worldScaleY);
            float safeScale = MathHelper.Min(safeScaleX, safeScaleY);
            float scale = MathHelper.Min(worldScale, safeScale);

            int width = (int)(scale * VirtualWidth + 0.5f);
            int height = (int)(scale * VirtualHeight + 0.5f);

            int x = clientBounds.Width / 2 - width / 2;
            int y = clientBounds.Height / 2 - height / 2;
            graphicsDevice.Viewport = new Viewport(x, y, width, height);
        }

        /// <inheritdoc/>
        public override Vector2 GetVectorPointOnScreen(Vector2 point)
        {
            Viewport viewport = graphicsDevice.Viewport;
            return base.GetVectorPointOnScreen(new Vector2(point.X - viewport.X, point.Y - viewport.Y));
        }

        /// <inheritdoc/>
        public override void Reset()
        {
            base.Reset();
            RecalculateBox();
        }
    }
}

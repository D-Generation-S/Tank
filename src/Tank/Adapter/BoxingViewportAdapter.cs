using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tank.Adapter
{
    class BoxingViewportAdapter : ScalingViewportAdapter
    {
        private readonly GameWindow window;
        private readonly int horizontalBleed;
        private readonly int verticalBleed;

        public BoxingViewportAdapter(GameWindow window, GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight) : this(window, graphicsDevice, virtualWidth, virtualHeight, 0, 0)
        {

        }

        public BoxingViewportAdapter(GameWindow window, GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight, int horizontalBleed, int verticalBleed) : base(graphicsDevice, virtualWidth, virtualHeight)
        {
            this.window = window;
            window.ClientSizeChanged += (sender, data) => OnClienteSizeChanged();
            this.horizontalBleed = horizontalBleed;
            this.verticalBleed = verticalBleed;
            RecalculateBox();
        }

        public void OnClienteSizeChanged()
        {
            RecalculateBox();
        }


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

        public override Vector2 GetVectorPointOnScreen(Vector2 point)
        {
            Viewport viewport = graphicsDevice.Viewport;
            return base.GetVectorPointOnScreen(new Vector2(point.X - viewport.X, point.Y - viewport.Y));
        }

        public override void Reset()
        {
            base.Reset();
            RecalculateBox();
        }
    }
}

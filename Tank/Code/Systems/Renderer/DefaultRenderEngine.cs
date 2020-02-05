using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.Components;
using Tank.Interfaces.Implementations;
using Tank.Interfaces.System;

namespace Tank.Code.Systems.Renderer
{
    class DefaultRenderEngine : IRenderEngine
    {
        private readonly List<IRenderer> renderObjects;
        private readonly SpriteBatch spriteBatch;

        private int drawOrder;
        public int DrawOrder => drawOrder;

        private bool visible;
        public bool Visible => visible;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public DefaultRenderEngine(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            renderObjects = new List<IRenderer>();
        }

        public void AddRenderer(IVisible visibleEntity)
        {
            AddRenderer(visibleEntity.Renderer);
        }

        public void AddRenderer(IRenderer renderer)
        {
            renderObjects.Add(renderer);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront);
            for (int renderIndex = 0; renderIndex < renderObjects.Count; renderIndex++)
            {
                IRenderer obj = renderObjects[renderIndex];
                spriteBatch.Draw(obj.Texture, obj.DrawingContainer, Color.White);
                renderObjects[renderIndex].DrawStep();
            }
            spriteBatch.End();
        }
    }
}

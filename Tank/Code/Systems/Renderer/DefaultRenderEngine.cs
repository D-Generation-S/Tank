using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses;
using Tank.Interfaces.Components;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;
using Tank.Interfaces.System;

namespace Tank.Code.Systems.Renderer
{
    class DefaultRenderEngine : BaseEntity, IRenderEngine
    {
        private readonly List<IVisibleEntity> renderObjects;
        private readonly SpriteBatch spriteBatch;
        private readonly GraphicsDevice graphicsDevice;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public DefaultRenderEngine(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            renderObjects = new List<IVisibleEntity>();
        }

        public override void Initzialize(string uniqueName)
        {
            active = true;
            alive = true;
            base.Initzialize(uniqueName);
        }

        public string AddEntity(IEntity entity)
        {
            if (entity.UniqueName == String.Empty)
            {
                return String.Empty;
            }

            if (entity is IVisibleEntity)
            {
                renderObjects.Add(((IVisibleEntity)entity));
                return entity.UniqueName;
            }

            return String.Empty;
        }

        public bool RemoveEntity(string entityName)
        {
            int counter = renderObjects.RemoveAll(entity => entity.UniqueName == entityName);

            return counter > 0;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront);
            graphicsDevice.Clear(Color.CornflowerBlue);
            for (int renderIndex = 0; renderIndex < renderObjects.Count; renderIndex++)
            {
                IRenderer obj = renderObjects[renderIndex].Renderer;
                if (obj.IsReady)
                {
                    spriteBatch.Draw(obj.Texture, obj.Destination, obj.Source, Color.White, 0, new Vector2(0,0), SpriteEffects.None, 1f);
                    obj.DrawStep(gameTime);
                }
                
            }
            spriteBatch.End();
        }
    }
}

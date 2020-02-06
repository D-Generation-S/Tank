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
        private readonly List<IRenderer> renderObjects;
        private readonly SpriteBatch spriteBatch;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public DefaultRenderEngine(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            renderObjects = new List<IRenderer>();
        }

        public override void Initzialize(string uniqueName)
        {
            active = true;
            base.Initzialize(uniqueName);
        }

        public string AddEntity(IEntity entity)
        {
            if (entity.UniqueName == String.Empty)
            {
                return "";
            }

            if (entity is IVisible)
            {
                renderObjects.Add(((IVisible)entity).Renderer);
                return entity.UniqueName;
            }

            if (entity is IRenderer)
            {
                renderObjects.Add(((IRenderer)entity));
                return entity.UniqueName;
            }

            return "";
        }

        public bool RemoveEntity(string entityName)
        {
            throw new NotImplementedException();
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses.Entites;
using Tank.Code.BaseClasses.Systems;
using Tank.Code.Screenmanager.ViewPortAdapters;
using Tank.Interfaces.Components;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;
using Tank.Interfaces.System;

namespace Tank.Code.Systems.Renderer
{
    class DefaultRenderEngine : BaseSystem, IRenderEngine
    {
        private readonly List<IVisibleEntity> renderObjects;
        private readonly SpriteBatch spriteBatch;
        private readonly GraphicsDevice graphicsDevice;
        private readonly BoxingViewportAdapter adapter;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public DefaultRenderEngine(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            renderObjects = new List<IVisibleEntity>();
            adapter = null;
        }

        public DefaultRenderEngine(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, BoxingViewportAdapter adapter)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            this.adapter = adapter;
            renderObjects = new List<IVisibleEntity>();
        }

        public override void Initzialize(string uniqueName)
        {
            active = true;
            alive = true;
            base.Initzialize(uniqueName);
        }

        public override string AddEntity(IEntity entity)
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

        public override bool RemoveEntity(string entityName)
        {
            int counter = renderObjects.RemoveAll(entity => entity.UniqueName == entityName);

            return counter > 0;
        }

        public void Draw(GameTime gameTime)
        {
            if (adapter == null)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, null);
            } else
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, adapter.GetScaleMatrix());
            }
            
            graphicsDevice.Clear(Color.CornflowerBlue);
            for (int renderIndex = 0; renderIndex < renderObjects.Count; renderIndex++)
            {
                IVisibleEntity entity = renderObjects[renderIndex];
                IRenderer renderer = entity.Renderer;
                if (renderer.IsReady)
                {
                    Rectangle realDestination = renderer.Destination;
                    realDestination.X += (int)entity.RotationAxis.X;
                    realDestination.Y += (int)entity.RotationAxis.Y;
                    spriteBatch.Draw(renderer.Texture, realDestination, renderer.Source, Color.White, entity.Rotation, entity.RotationAxis, SpriteEffects.None, 1f);
                    renderer.DrawStep(gameTime);
                }
                
            }
            spriteBatch.End();
        }
    }
}

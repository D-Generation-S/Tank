using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Components;
using Tank.src.Events;
using Tank.src.Events.ComponentBased;
using Tank.src.Events.EntityBased;
using Tank.src.Interfaces.EntityComponentSystem;
using Tank.src.Interfaces.EntityComponentSystem.Manager;
using Tank.src.Validator;

namespace Tank.src.Systems
{
    class RenderSystem : AbstractSystem
    {
        private readonly SpriteBatch spriteBatch;

        public RenderSystem(SpriteBatch spriteBatch) : base()
        {
            this.spriteBatch = spriteBatch;
            validators.Add(new RenderableEntityValidator());
        }

        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
        }

        public override void EventNotification(object sender, EventArgs eventArgs)
        {
            base.EventNotification(sender, eventArgs);
            EntityRemoved(eventArgs);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (uint entityId in watchedEntities)
            {
                PlaceableComponent placeableComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
                VisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);
                Rectangle destination = visibleComponent.Destination;
                destination.X = (int)placeableComponent.Position.X;
                destination.Y = (int)placeableComponent.Position.Y;
                visibleComponent.Destination = destination;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (uint entityId in watchedEntities)
            {
                if (entitiesToRemove.Contains(entityId))
                {
                    continue;
                }
                PlaceableComponent placeableComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
                VisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);
                if (visibleComponent.Texture == null)
                {
                    continue;
                }
                spriteBatch.Draw(visibleComponent.Texture, visibleComponent.Destination, visibleComponent.Source, visibleComponent.Color, placeableComponent.Rotation, Vector2.Zero, SpriteEffects.None, 1f);
            }
        }
    }
}

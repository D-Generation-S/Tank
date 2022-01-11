using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Tank.Components;
using Tank.Validator;
using TankEngine.EntityComponentSystem.Events;
using TankEngine.EntityComponentSystem.Systems;

namespace Tank.Systems
{
    /// <summary>
    /// System to bind entities together
    /// </summary>
    class BindingSystem : AbstractSystem
    {
        private List<uint> entitesToRemove;

        /// <summary>
        /// Create a new instance of the binding system
        /// </summary>
        public BindingSystem() : base()
        {
            validators.Add(new BindEntityValidator());
            entitesToRemove = new List<uint>();
        }


        /// <inheritdoc/>
        public override void EventNotification(object sender, IGameEvent eventArgs)
        {
            base.EventNotification(sender, eventArgs);
            if (eventArgs is EntityRemovedEvent entityRemovedEvent)
            {
                entityManager.GetEntitiesWithComponent<BindComponent>().ForEach(entity =>
                {
                    BindComponent component = entityManager.GetComponent<BindComponent>(entity);
                    if (component.BoundEntityId == entityRemovedEvent.EntityId && component.DeleteIfParentGone && !entitesToRemove.Contains(entityRemovedEvent.EntityId))
                    {
                        entitesToRemove.Add(component.EntityId);
                    }
                });
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            entitesToRemove.ForEach(entity =>
            {

                RemoveEntityEvent removeEntityEvent = eventManager.CreateEvent<RemoveEntityEvent>();
                removeEntityEvent.EntityId = entity;
                FireEvent(removeEntityEvent);
            });
            entitesToRemove.Clear();
            foreach (uint entity in watchedEntities)
            {
                PlaceableComponent basePosition = entityManager.GetComponent<PlaceableComponent>(entity);
                BindComponent binding = entityManager.GetComponent<BindComponent>(entity);
                if (binding == null || !binding.PositionBound || !entityManager.HasComponent(binding.BoundEntityId, typeof(PlaceableComponent)))
                {
                    continue;
                }

                PlaceableComponent bindingPosition = entityManager.GetComponent<PlaceableComponent>(binding.BoundEntityId);
                if (binding.Source)
                {
                    basePosition.Position = bindingPosition.Position + binding.Offset;
                    continue;
                }

                bindingPosition.Position = basePosition.Position + binding.Offset;
            }
        }
    }
}

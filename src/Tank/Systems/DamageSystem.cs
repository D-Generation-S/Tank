using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.Components;
using Tank.DataStructure;
using Tank.Events.EntityBased;
using Tank.Events.PhysicBased;
using Tank.Events.TerrainEvents;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Validator;

namespace Tank.Systems
{
    /// <summary>
    /// This system will handle any damage between entites
    /// </summary>
    class DamageSystem : AbstractSystem
    {
        /// <summary>
        /// Create a new instance of the damnage system
        /// </summary>
        public DamageSystem() : base()
        {
            validators.Add(new DamageEntityValidator());
        }

        /// <inheritdoc/>
        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);

            eventManager.SubscribeEvent(this, typeof(MapCollisionEvent));
        }

        /// <inheritdoc/>
        public override void EventNotification(object sender, EventArgs eventArgs)
        {
            base.EventNotification(sender, eventArgs);
            if (eventArgs is MapCollisionEvent)
            {
                MapCollisionEvent collisionEvent = (MapCollisionEvent)eventArgs;
                DamageComponent damageComponent = entityManager.GetComponent<DamageComponent>(collisionEvent.EntityId);

                if (damageComponent == null)
                {
                    return;
                }

                FireEvent(new RemoveEntityEvent(collisionEvent.EntityId));
                Effect(collisionEvent, damageComponent);
                DamageTerrain(damageComponent, collisionEvent);
            }
        }

        /// <summary>
        /// This method will fire an event if terrain should be damaged
        /// </summary>
        /// <param name="damageComponent">The component used for damage</param>
        /// <param name="collisionEvent">The event where the collision occured</param>
        private void DamageTerrain(DamageComponent damageComponent, MapCollisionEvent collisionEvent)
        {
            if (!damageComponent.DamageTerrain || damageComponent.DamageArea == null)
            {
                return;
            }
            Circle damageArea = damageComponent.DamageArea;
            damageArea.Center = collisionEvent.CollisionPosition;

            FireEvent(new DamageTerrainEvent(damageArea));
        }

        /// <summary>
        /// This method will create an explosion effect if needed
        /// </summary>
        /// <param name="collisionEvent">The event of the collision</param>
        /// <param name="damageComponent">The component containing the damage data</param>
        private void Effect(MapCollisionEvent collisionEvent, DamageComponent damageComponent)
        {
            if (!damageComponent.Effect)
            {
                return;
            }
            List<IComponent> components = damageComponent.EffectFactory.GetGameObjects();
            foreach (IComponent component in components)
            {
                if (component is PlaceableComponent placeable)
                {
                    Vector2 position = collisionEvent.CollisionPosition;
                    placeable.Position += position;
                }
            }
            FireEvent(new AddEntityEvent(components));
        }
    }
}

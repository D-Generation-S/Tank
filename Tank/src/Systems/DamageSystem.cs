using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.src.Components;
using Tank.src.DataStructure;
using Tank.src.Events.EntityBased;
using Tank.src.Events.PhysicBased;
using Tank.src.Events.TerrainEvents;
using Tank.src.Interfaces.EntityComponentSystem;
using Tank.src.Interfaces.EntityComponentSystem.Manager;
using Tank.src.Interfaces.Factories;
using Tank.src.Validator;

namespace Tank.src.Systems
{
    /// <summary>
    /// This system will handle any damage between entites
    /// </summary>
    class DamageSystem : AbstractSystem
    {
        /// <summary>
        /// The factory to use to generate explosion effects
        /// </summary>
        private readonly IGameObjectFactory exlosionFactory;

        /// <summary>
        /// Create a new instance of the damnage system
        /// </summary>
        /// <param name="exlosionFactory">The factory to use for creating explosions</param>
        public DamageSystem(IGameObjectFactory exlosionFactory) : base()
        {
            validators.Add(new DamageEntityValidator());
            this.exlosionFactory = exlosionFactory;
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
                FireEvent<RemoveEntityEvent>(new RemoveEntityEvent(collisionEvent.EntityId));
                Explosion(collisionEvent, damageComponent);
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
            if (!damageComponent.DamangeTerrain)
            {
                return;
            }
            Circle damageArea = damageComponent.DamageArea;
            damageArea.Center.X += collisionEvent.CollisionPosition.X;
            damageArea.Center.Y += collisionEvent.CollisionPosition.Y;

            FireEvent<DamageTerrainEvent>(new DamageTerrainEvent(damageArea));
        }

        /// <summary>
        /// This method will create an explosion effect if needed
        /// </summary>
        /// <param name="collisionEvent">The event of the collision</param>
        /// <param name="damageComponent">The component containing the damage data</param>
        private void Explosion(MapCollisionEvent collisionEvent, DamageComponent damageComponent)
        {
            if (!damageComponent.Explosive)
            {
                return;
            }
            List<IComponent> components = exlosionFactory.GetGameObjects();
            foreach (IComponent component in components)
            {
                if (component is PlaceableComponent)
                {
                    Vector2 explosionPosition = new Vector2(collisionEvent.CollisionPosition.X, collisionEvent.CollisionPosition.Y);
                    ((PlaceableComponent)component).Position = explosionPosition;
                }
            }
            FireEvent<AddEntityEvent>(new AddEntityEvent(components));
        }
    }
}

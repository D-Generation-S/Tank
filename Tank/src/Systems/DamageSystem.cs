using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class DamageSystem : AbstractSystem
    {
        private readonly IGameObjectFactory exlosionFactory;

        public DamageSystem(IGameObjectFactory exlosionFactory) : base()
        {
            validators.Add(new DamageEntityValidator());
            this.exlosionFactory = exlosionFactory;
        }

        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);

            eventManager.SubscribeEvent(this, typeof(MapCollisionEvent));
        }

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

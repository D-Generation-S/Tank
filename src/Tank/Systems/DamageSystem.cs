﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Tank.Components;
using Tank.Components.GameObject;
using Tank.Components.Tags;
using Tank.Events.PhysicBased;
using Tank.Events.TerrainEvents;
using Tank.Validator;
using TankEngine.DataStructures.Geometrics;
using TankEngine.EntityComponentSystem;
using TankEngine.EntityComponentSystem.Events;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Systems;

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
        public override void EventNotification(object sender, IGameEvent eventArgs)
        {
            base.EventNotification(sender, eventArgs);
            if (eventArgs is MapCollisionEvent collisionEvent)
            {
                DamageComponent damageComponent = entityManager.GetComponent<DamageComponent>(collisionEvent.EntityId);

                if (damageComponent == null)
                {
                    return;
                }

                Effect(collisionEvent, damageComponent);
                DamageTerrain(damageComponent, collisionEvent);
                DamageGameObjects(damageComponent, collisionEvent);

                RemoveEntityEvent removeEntityEvent = eventManager.CreateEvent<RemoveEntityEvent>();
                removeEntityEvent.EntityId = collisionEvent.EntityId;
                FireEvent(removeEntityEvent);
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
            damageArea.Center = collisionEvent.Position;
            DamageTerrainEvent damageTerrainEvent = CreateEvent<DamageTerrainEvent>();
            damageTerrainEvent.DamageArea = damageArea;
            FireEvent(damageTerrainEvent);
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
            List<IComponent> components = damageComponent.EffectFactory.GetNewObject();
            foreach (IComponent component in components)
            {
                if (component is PlaceableComponent placeable)
                {
                    placeable.Position = collisionEvent.Position;
                }
            }
            AddEntityEvent addEntiyEvent = CreateEvent<AddEntityEvent>();
            addEntiyEvent.Components = components;
            FireEvent(addEntiyEvent);
        }

        /// <summary>
        /// Damage a game object with a given value
        /// </summary>
        /// <param name="damageComponent">The damage component to use</param>
        /// <param name="collisionEvent">The collision event</param>
        private void DamageGameObjects(DamageComponent damageComponent, MapCollisionEvent collisionEvent)
        {
            List<uint> objects = entityManager.GetEntitiesWithComponent<GameObjectTag>();

            BindComponent projectileBindComponent = entityManager.GetComponent<BindComponent>(collisionEvent.EntityId);
            PlayerStatisticComponent playerStatisticComponent = null;
            if (projectileBindComponent != null)
            {
                entityManager.GetEntitiesWithComponent<PlayerStatisticComponent>()
                             .Select(id => entityManager.GetComponent<BindComponent>(id))
                             .Where(bind => bind != null)
                             .Where(bindComponent => bindComponent.BoundEntityId == projectileBindComponent.BoundEntityId)
                             .Select(bindComponent => entityManager.GetComponent<PlayerStatisticComponent>(bindComponent.EntityId))
                             .FirstOrDefault();
            }

            foreach (uint gameObject in objects)
            {
                PlaceableComponent placeableComponent = entityManager.GetComponent<PlaceableComponent>(gameObject);
                ColliderComponent collider = entityManager.GetComponent<ColliderComponent>(gameObject);
                Rectangle collisionArea = collider == null ? Rectangle.Empty : collider.Collider;
                GameObjectData gameObjectData = entityManager.GetComponent<GameObjectData>(gameObject);

                if (placeableComponent == null || collider == null || gameObjectData == null)
                {
                    continue;
                }

                collisionArea.Location += placeableComponent.Position.ToPoint();
                List<Point> points = new List<Point>();
                points.Add(new Point(collisionArea.Center.X, collisionArea.Top));
                points.Add(new Point(collisionArea.Center.X, collisionArea.Bottom));
                points.Add(new Point(collisionArea.Left, collisionArea.Y));
                points.Add(new Point(collisionArea.Right, collisionArea.Y));
                points.Add(collisionArea.Center);
                Point collisionPoint = points.Where(dot => damageComponent.DamageArea.IsInInCircle(dot))
                                             .OrderBy(dot => Vector2.Distance(dot.ToVector2(), damageComponent.DamageArea.Center))
                                             .FirstOrDefault();
                if (collisionPoint == Point.Zero)
                {
                    continue;
                }

                float armor = gameObjectData.Properties.ContainsKey("Armor") ? gameObjectData.Properties["Armor"] : 0;
                float damageToTake = damageComponent.CenterDamageValue;
                Vector2 centerDistance = damageComponent.DamageArea.Center - placeableComponent.Position;
                float damangeReduction = (float)damageComponent.CenterDamageValue * (centerDistance.Length() / (float)damageComponent.DamageArea.Diameter);
                damageToTake = damageComponent.CenterDamageValue - damangeReduction;
                float reflectedDamage = damageComponent.CenterDamageValue * armor / 100;
                damageToTake -= reflectedDamage;
                if (damageToTake < 0 || !gameObjectData.Properties.ContainsKey("Health"))
                {
                    continue;
                }

                gameObjectData.Properties["Health"] -= damageToTake;
                gameObjectData.DataChanged = true;
                if (playerStatisticComponent != null && projectileBindComponent != null)
                {
                    int pointsToAdd = (int)damageToTake;
                    if (projectileBindComponent.BoundEntityId == gameObjectData.EntityId)
                    {
                        pointsToAdd *= -1;
                    }
                    playerStatisticComponent.Points += pointsToAdd;
                }
                if (gameObjectData.Properties["Health"] <= 0)
                {
                    if (playerStatisticComponent != null)
                    {
                        playerStatisticComponent.Kills++;
                    }
                    RemoveEntityEvent removeEntityEvent = eventManager.CreateEvent<RemoveEntityEvent>();
                    removeEntityEvent.EntityId = gameObjectData.EntityId;
                    FireEvent(removeEntityEvent);
                }

            }
        }
    }
}


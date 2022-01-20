using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tank.Components;
using Tank.Components.GameObject;
using Tank.Components.Tags;
using Tank.Interfaces.Builders;
using Tank.Register;
using Tank.Validator;
using TankEngine.EntityComponentSystem;
using TankEngine.EntityComponentSystem.Components.World;
using TankEngine.EntityComponentSystem.Events;
using TankEngine.EntityComponentSystem.Systems;

namespace Tank.Systems
{
    class ProjectileSpawnSystem : AbstractSystem
    {
        private readonly Register<IGameObjectBuilder> register;

        public ProjectileSpawnSystem(Register<IGameObjectBuilder> register)
        {
            this.register = register;
            validators.Add(new ProjectileSpawnValidator());
        }

        /// <inheritdoc/>
        public override void PhysicUpdate(GameTime gameTime)
        {
            foreach (uint entityId in watchedEntities)
            {
                PositionComponent placeableComponent = entityManager.GetComponent<PositionComponent>(entityId);
                ProjectileSpawnComponent spawnComponent = entityManager.GetComponent<ProjectileSpawnComponent>(entityId);

                if (placeableComponent == null || spawnComponent == null)
                {
                    entityManager.RemoveComponents(entityId, typeof(RoundBlockingTag));
                    continue;
                }
                if (spawnComponent.Amount <= 0)
                {
                    RemoveEntityEvent removeEntityEvent = eventManager.CreateEvent<RemoveEntityEvent>();
                    removeEntityEvent.EntityId = entityId;
                    FireEvent(removeEntityEvent);
                    continue;
                }

                spawnComponent.CurrentTick++;
                if (spawnComponent.CurrentTick < spawnComponent.TicksUntilSpawn)
                {
                    continue;
                }
                spawnComponent.CurrentTick -= spawnComponent.TicksUntilSpawn;

                IGameObjectBuilder builder = register.GetValue(spawnComponent.ProjectileToSpawn);
                List<IComponent> components = builder.BuildGameComponents();

                PositionComponent position = (PositionComponent)components.Find(component => component.Type == typeof(PositionComponent));
                MoveableComponent moveableComponent = (MoveableComponent)components.Find(component => component.Type == typeof(MoveableComponent));

                position.Position = GetSpawnPosition(placeableComponent, spawnComponent);
                Vector2 velocity = Vector2.Zero;
                velocity += Vector2.UnitY * (float)Math.Sin(placeableComponent.Rotation);
                velocity += Vector2.UnitX * (float)Math.Cos(placeableComponent.Rotation);
                velocity.Normalize();
                Vector2 offset = velocity * spawnComponent.Offset;
                position.Position += offset;
                velocity *= spawnComponent.Strenght;
                moveableComponent.Velocity = velocity;
                if (spawnComponent.UseParentEntity)
                {
                    components.Add(entityManager.CreateComponent<RoundBlockingTag>());
                    BindComponent parent = entityManager.CreateComponent<BindComponent>();
                    parent.BoundEntityId = spawnComponent.ParentEntity;
                    components.Add(parent);
                }

                AddEntityEvent addEntityEvent = eventManager.CreateEvent<AddEntityEvent>();
                addEntityEvent.Components = components;
                FireEvent(addEntityEvent);

                spawnComponent.Amount -= 1;
            }
        }

        private Vector2 GetSpawnPosition(PositionComponent placeableComponent, ProjectileSpawnComponent spawnComponent)
        {
            Vector2 returnPosition = spawnComponent.UseParentEntity ? GetReferingPosition(placeableComponent, spawnComponent) : GetSimplePosition(placeableComponent);
            return returnPosition;
        }

        private Vector2 GetSimplePosition(PositionComponent placeableComponent)
        {
            return placeableComponent.Position;
        }

        private Vector2 GetReferingPosition(PositionComponent placeableComponent, ProjectileSpawnComponent spawnComponent)
        {
            Debug.WriteLine("Not implemented!");
            return GetSimplePosition(placeableComponent);
        }

    }
}

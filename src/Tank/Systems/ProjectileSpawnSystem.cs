using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tank.Components;
using Tank.Components.GameObject;
using Tank.Components.Tags;
using Tank.Events.EntityBased;
using Tank.Interfaces.Builders;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Register;
using Tank.Validator;

namespace Tank.Systems
{
    class ProjectileSpawnSystem : AbstractSystem
    {
        private readonly Register<IGameObjectBuilder> register;

        /// <summary>
        /// A fixed number used as to find out how many physic updates are needed
        /// </summary>
        private readonly float fixedDeltaTime;

        /// <summary>
        /// The time from the last call
        /// </summary>
        private float previousTime;

        /// <summary>
        /// The time left over from the last physic calculation
        /// </summary>
        private float leftOverDeltaTime;

        public ProjectileSpawnSystem(Register<IGameObjectBuilder> register)
        {
            fixedDeltaTime = 16;

            this.register = register;
            validators.Add(new ProjectileSpawnValidator());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float deltaTime = gameTime.TotalGameTime.Milliseconds - previousTime;
            previousTime = gameTime.TotalGameTime.Milliseconds;
            int timeSteps = (int)((deltaTime + leftOverDeltaTime) / fixedDeltaTime);

            foreach (uint entityId in watchedEntities)
            {
                PlaceableComponent placeableComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
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

                for (int i = 0; i < timeSteps; i++)
                {
                    spawnComponent.CurrentTick++;
                }
                if (spawnComponent.CurrentTick < spawnComponent.TicksUntilSpawn)
                {
                    continue;
                }
                spawnComponent.CurrentTick -= spawnComponent.TicksUntilSpawn;

                IGameObjectBuilder builder = register.GetValue(spawnComponent.ProjectileToSpawn);
                List<IComponent> components = builder.BuildGameComponents();

                PlaceableComponent position = (PlaceableComponent)components.Find(component => component.Type == typeof(PlaceableComponent));
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
                }

                AddEntityEvent addEntityEvent = eventManager.CreateEvent<AddEntityEvent>();
                addEntityEvent.Components = components;
                FireEvent(addEntityEvent);

                //Vector2 projectileSpawnLocation = 

                spawnComponent.Amount -= 1;

            }
        }

        private Vector2 GetSpawnPosition(PlaceableComponent placeableComponent, ProjectileSpawnComponent spawnComponent)
        {
            Vector2 returnPosition = spawnComponent.UseParentEntity ? GetReferingPosition(placeableComponent, spawnComponent) : GetSimplePosition(placeableComponent);
            return returnPosition;
        }

        private Vector2 GetSimplePosition(PlaceableComponent placeableComponent)
        {
            return placeableComponent.Position;
        }

        private Vector2 GetReferingPosition(PlaceableComponent placeableComponent, ProjectileSpawnComponent spawnComponent)
        {
            Debug.WriteLine("Not implemented!");
            return GetSimplePosition(placeableComponent);
        }

    }
}

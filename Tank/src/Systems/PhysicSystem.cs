using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;
using Tank.src.Components;
using Tank.src.Events.EntityBased;
using Tank.src.Validator;

namespace Tank.src.Systems
{
    class PhysicSystem : AbstractSystem
    {
        private readonly float fixedDeltaTime;
        private readonly float fixedDeltaTimeSeconds;
        private float leftOverDeltaTime;

        private float currentTime;
        private float previousTime;

        private Rectangle screenBound;

        public PhysicSystem(Rectangle screenBound) : base()
        {
            fixedDeltaTime = 16;
            fixedDeltaTimeSeconds = fixedDeltaTime / 1000f;
            validators.Add(new PhysicEntityValidator());
            this.screenBound = screenBound;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            currentTime = gameTime.TotalGameTime.Milliseconds;

            float deltaTime = currentTime - previousTime;
            previousTime = currentTime;

            int timeSteps = (int)((deltaTime + leftOverDeltaTime) / fixedDeltaTime);

            leftOverDeltaTime = deltaTime - (timeSteps * fixedDeltaTime);
            List<uint> entitesToRemove = new List<uint>();

            for (int iteration = 0; iteration < timeSteps; iteration++)
            {
                foreach (uint entityId in watchedEntities)
                {
                    PlaceableComponent placeComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
                    MoveableComponent moveComponent = entityManager.GetComponent<MoveableComponent>(entityId);
                    if (moveComponent.OnGround)
                    {
                        continue;
                    }
                    Vector2 objVelocity = moveComponent.Velocity;

                    objVelocity.Y += 980 * fixedDeltaTimeSeconds;
                    objVelocity.X += objVelocity.X * fixedDeltaTimeSeconds;

                    Vector2 oldPosition = placeComponent.Position;
                    placeComponent.Position += objVelocity;
                    placeComponent.Rotation = (float)Math.Atan2(placeComponent.Position.Y - oldPosition.Y, placeComponent.Position.X - oldPosition.X);

                    if (!screenBound.Contains(placeComponent.Position))
                    {
                        entitesToRemove.Add(entityId);
                    }
                }
            }

            foreach (uint entityId in entitesToRemove)
            {
                FireEvent<RemoveEntityEvent>(new RemoveEntityEvent(entityId));
                EntityRemoved(entityId);
            }
        }
    }
}

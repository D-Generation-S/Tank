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
    /// <summary>
    /// This system will performe physic actions on the entites
    /// </summary>
    class PhysicSystem : AbstractSystem
    {
        /// <summary>
        /// A fixed number used as to find out how many physic updates are needed
        /// </summary>
        private readonly float fixedDeltaTime;

        /// <summary>
        /// The milliseconds which are spend for an update
        /// </summary>
        private readonly float fixedDeltaTimeSeconds;

        /// <summary>
        /// The time left over from the last physic calculation
        /// </summary>
        private float leftOverDeltaTime;

        /// <summary>
        /// The time from the last call
        /// </summary>
        private float previousTime;

        /// <summary>
        /// The screen rectangle, entites leaving the area will be removed
        /// </summary>
        private Rectangle screenBound;

        /// <summary>
        /// Create a new instance of the physic system with a given screen bound
        /// </summary>
        /// <param name="screenBound"></param>
        public PhysicSystem(Rectangle screenBound) : base()
        {
            fixedDeltaTime = 16;
            fixedDeltaTimeSeconds = fixedDeltaTime / 1000f;
            validators.Add(new PhysicEntityValidator());
            this.screenBound = screenBound;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float deltaTime = gameTime.TotalGameTime.Milliseconds - previousTime;
            previousTime = gameTime.TotalGameTime.Milliseconds;

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

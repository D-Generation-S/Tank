using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.Code.BaseClasses;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;

namespace Tank.Code.Systems.Physic
{
    class DefaultPhysicEngine : BaseEntity, ISystem
    {
        private readonly List<IPhysicEntity> objects;

        private readonly float fixedDeltaTime;
        private readonly float fixedDeltaTimeSeconds;
        private float leftOverDeltaTime;

        private float currentTime;
        private float previousTime;

        private bool enabled;
        public bool Enabled => enabled;

        private int updateOrder;
        public int UpdateOrder => updateOrder;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public DefaultPhysicEngine()
        {
            objects = new List<IPhysicEntity>();

            fixedDeltaTime = 16;
            fixedDeltaTimeSeconds = fixedDeltaTime / 1000f;
        }

        public override void Initzialize(string uniqueName)
        {
            active = true;
            base.Initzialize(uniqueName);
        }

        public string AddEntity(IEntity entity)
        {
            if (entity.UniqueName == String.Empty)
            {
                return String.Empty;
            }

            if (entity is IPhysicEntity)
            {
                objects.Add((IPhysicEntity)entity);
            }

            return entity.UniqueName;
        }

        public bool RemoveEntity(string entityName)
        {
            int counter = objects.RemoveAll(entity => entity.UniqueName == entityName);

            return counter > 0;
        }

        public override void Update(GameTime gameTime)
        {
            currentTime = gameTime.TotalGameTime.Milliseconds;

            float deltaTime = currentTime - previousTime;
            previousTime = currentTime;

            int timeSteps = (int)((deltaTime + leftOverDeltaTime) / fixedDeltaTime);

            leftOverDeltaTime = deltaTime - (timeSteps * fixedDeltaTime);

            for (int iteration = 0; iteration < timeSteps; iteration++)
            {
                for (int objectIndex = 0; objectIndex < objects.Count; objectIndex++)
                {
                    IPhysicEntity obj = objects[objectIndex];
                    Vector2 objVelocity = obj.Velocity;

                    objVelocity.Y += 980 * fixedDeltaTimeSeconds;
                    objVelocity.X += objVelocity.X * fixedDeltaTimeSeconds;

                    obj.Position += objVelocity;
                }
            }

        }
    }
}

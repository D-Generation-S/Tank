using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.Interfaces.Entity;
using Tank.Interfaces.System;

namespace Tank.Code.Systems.Physic
{
    class DefaultPhysicEngine : IPhysicEngine
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

        public void AddPhysicObject(IPhysicEntity physicEntity)
        {
            objects.Add(physicEntity);
        }

        public void Update(GameTime gameTime)
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

                    objVelocity.X += 980 * fixedDeltaTimeSeconds;

                    obj.Velocity = objVelocity;
                }
            }

        }
    }
}

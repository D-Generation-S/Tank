using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.Code.BaseClasses;
using Tank.Interfaces.Components;
using Tank.Interfaces.Entity;
using Tank.Interfaces.System;

namespace Tank.Code.Systems.Physic
{
    class DefaultPhysicEngine : BaseEntity, IPhysicEngine
    {
        private readonly List<IMoveable> objects;

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
            objects = new List<IMoveable>();

            fixedDeltaTime = 16;
            fixedDeltaTimeSeconds = fixedDeltaTime / 1000f;
        }

        public string AddEntity(IEntity entity)
        {
            if (entity.UniqueName == String.Empty)
            {
                return "";
            }

            if (entity is IMoveable)
            {
                objects.Add((IMoveable)entity);
            }

            return entity.UniqueName;
        }

        public bool RemoveEntity(string entityName)
        {
            return true;
        }

        public void AddPhysicObject(IEntity entity)
        {

            
        }

        public void AddPhysicObject(IMoveable moveable)
        {
            objects.Add(moveable);
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
                    IMoveable obj = objects[objectIndex];
                    Vector2 objVelocity = obj.Velocity;

                    objVelocity.Y += 980 * fixedDeltaTimeSeconds;
                    objVelocity.X += objVelocity.X * fixedDeltaTimeSeconds;

                    obj.Velocity = objVelocity;
                }
            }

        }
    }
}

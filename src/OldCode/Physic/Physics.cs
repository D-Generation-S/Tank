using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Tank.Code.Entities;
using Tank.Interfaces;

namespace Tank.Code
{
    public class Physics
    {
        private const int fixedDeltaTime = 16;
        float previousTime;
        float currentTime;
        float fixedDeltaTimeSeconds = fixedDeltaTime / 1000.0f;
        float leftOverDeltaTime = 0;

        public List<IPhysicsObj> objects;

        private static Physics instance;

        // Constructor
        private Physics()
        {
            objects = new List<IPhysicsObj>();
        }

        public static Physics Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Physics();
                }
                return instance;
            }
        }

        public void Add(IPhysicsObj obj)
        {
            objects.Insert(new Random().Next(objects.Count()), obj);
        }
        public void Remove(IPhysicsObj obj)
        {
            objects.Remove(obj);
        }

        public void RemoveAllPhysicPixels()
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].GetType() == typeof(DynamicPixel))
                {
                    Remove(objects[i]);
                }
            }
        }


        public void Update(GameTime gameTime)
        {
            // This game uses fixed-sized timesteps.
            // The amount of time elapsed since last update is split up into units of 16 ms
            // any left over is pushed over to the next update
            // we take those units of 16 ms and update the simulation that many times.
            // a fixed timestep will make collision detection and handling (in the Player class, esp.) a lot simpler
            // A low framerate will not compromise any collision detections, while it'll still run at a consistent speed. 

            currentTime = gameTime.TotalGameTime.Milliseconds;
            float deltaTimeMS = currentTime - previousTime; // how much time has elapsed since the last update

            previousTime = currentTime; // reset previousTime

            // Find out how many timesteps we can fit inside the elapsed time
            int timeStepAmt = ((int)(deltaTimeMS + leftOverDeltaTime) / fixedDeltaTime);

            // Limit the timestep amount to prevent freezing
            timeStepAmt = MathHelper.Min(timeStepAmt, 1);

            // store left over time for the next frame
            leftOverDeltaTime = deltaTimeMS - (timeStepAmt * fixedDeltaTime);

            for (int iteration = 1; iteration <= timeStepAmt; iteration++)
            {
                for (int i = 0; i < objects.Count(); i++)
                { // loop through every PhysicsObj

                    IPhysicsObj obj = objects[i];
                    // get their velocity
                    float velX = obj.velX;
                    float velY = obj.velY;

                    // add gravity
                    velY += 980 * fixedDeltaTimeSeconds;
                    obj.velY = velY;

                    // Always add x velocity
                    obj.x = obj.x + velX * fixedDeltaTimeSeconds;



                    // if it's a player, only add y velocity if he's not on the ground.
                    if (obj.GetType() == typeof(Vehicle))
                    {
                        if (!(((Vehicle)obj).IsOnGround && velY > 0))
                            obj.y = (obj.y + velY * fixedDeltaTimeSeconds);
                    }
                    else
                        obj.y = (obj.y + velY * fixedDeltaTimeSeconds);

                    // allow the object to do collision detection and other things
                    obj.checkConstraints(gameTime);
                }
            }
        }
    }
}

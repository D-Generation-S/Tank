using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tank.Interfaces
{
    public interface IPhysicsObj
    {
        float velX
        {
            get;
            set;
        }
        float velY
        {
            get;
            set;
        }

        float x
        {
            get;
            set;
        }
        float y
        {
            get;
            set;
        }

        void checkConstraints(GameTime CurrentGameTime);
    }
}
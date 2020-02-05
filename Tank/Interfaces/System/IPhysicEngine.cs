using Microsoft.Xna.Framework;
using Tank.Interfaces.Components;
using Tank.Interfaces.Entity;

namespace Tank.Interfaces.System
{
    interface IPhysicEngine : IUpdateable
    {
        void AddPhysicObject(IEntity moveable);

        void AddPhysicObject(IMoveable moveable);
    }
}

using Microsoft.Xna.Framework;
using Tank.Interfaces.Components;

namespace Tank.Interfaces.System
{
    interface IPhysicEngine : IUpdateable
    {
        void AddPhysicObject(IMoveable moveable);
    }
}

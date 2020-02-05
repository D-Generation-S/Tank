using Microsoft.Xna.Framework;
using Tank.Interfaces.Entity;

namespace Tank.Interfaces.System
{
    interface IPhysicEngine : IUpdateable
    {
        void AddPhysicObject(IPhysicEntity physicEntity);
    }
}

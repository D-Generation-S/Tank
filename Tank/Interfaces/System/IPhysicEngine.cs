using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.Entity;

namespace Tank.Interfaces.System
{
    interface IPhysicEngine : IUpdateable
    {
        void AddPhysicObject(IPhysicEntity physicEntity);
    }
}

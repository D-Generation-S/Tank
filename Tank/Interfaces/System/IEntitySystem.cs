using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.Implementations;
using Tank.Interfaces.Entity;

namespace Tank.Interfaces.System
{
    interface IEntitySystem : IDrawableEntity, ISystem
    {
        //IList<ISystem> Systems { get; }

        IList<IEntity> Entities { get; }

        //IList<IDrawableEntity> DrawableEntities { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.Components;

namespace Tank.Interfaces.Entity
{
    interface ICollideableEntity : IEntity, ICollideable
    {
    }
}

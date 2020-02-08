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
        /// <summary>
        /// All the entites on the screen
        /// </summary>
        IList<IEntity> Entities { get; }
    }
}

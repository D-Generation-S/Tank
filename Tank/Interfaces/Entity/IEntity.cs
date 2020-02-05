using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Interfaces.Entity
{
    interface IEntity : IUpdateable
    {
        Vector2 Position { get; }

        void Reset();
    }
}

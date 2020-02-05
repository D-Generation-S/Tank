using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Interfaces.Components
{
    interface IMoveable
    {
        Vector2 Velocity { get; set; }
    }
}

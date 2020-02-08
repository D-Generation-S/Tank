﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Interfaces.Components
{
    interface ICollideable : IPlaceable
    {
        bool MapCollision { get; }

        Rectangle Collider { get; }
    }
}

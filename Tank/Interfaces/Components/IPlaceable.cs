﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Interfaces.Components
{
    interface IPlaceable
    {
        Vector2 Position { get; }
    }
}

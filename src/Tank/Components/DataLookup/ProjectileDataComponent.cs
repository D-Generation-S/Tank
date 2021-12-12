using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Components.DataLookup
{
    class ProjectileDataComponent : BaseComponent
    {
        public string Name;
        public int Position;
        public int Amount;
        public int TicksUntilSpawn;
        public override void Init()
        {
            Name = string.Empty;
            Position = -1;
            Amount = 0;
            TicksUntilSpawn = 1;
        }
    }
}

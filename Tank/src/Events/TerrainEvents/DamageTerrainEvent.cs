using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.DataStructure;

namespace Tank.src.Events.TerrainEvents
{
    class DamageTerrainEvent : EventArgs
    {
        private readonly Circle damageArea;
        public Circle DamageArea => damageArea;

        public DamageTerrainEvent(Circle damageArea)
        {
            this.damageArea = damageArea;
        }
    }
}

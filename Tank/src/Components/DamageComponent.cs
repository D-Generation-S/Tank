using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.DataStructure;

namespace Tank.src.Components
{
    class DamageComponent : BaseComponent
    {
        private readonly bool damageTerrain;
        public bool DamangeTerrain => damageTerrain;

        private readonly Circle damageArea;
        public Circle DamageArea => damageArea;

        private readonly int centerDamageValue;
        private int CenterDamageValue => centerDamageValue;

        public DamageComponent(bool damageTerrain, int centerDamage, Circle damageArea)
        {
            this.damageTerrain = damageTerrain;
            centerDamageValue = centerDamage;
            this.damageArea = damageArea;
        }
    }
}

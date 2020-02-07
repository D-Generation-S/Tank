using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses;
using Tank.Interfaces.Components;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;

namespace Tank.Code.Entities.Weapon
{
    class Projectile :DrawableEntity, IPhysicEntity
    {
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public Projectile(IRenderer renderer) : base(renderer)
        {

        }

        public override void Initzialize(string uniqueName)
        {
            base.Initzialize(uniqueName);
            active = true;
            alive = true;
            
        }
    }
}

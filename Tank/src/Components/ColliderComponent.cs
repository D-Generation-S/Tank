using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Components
{
    class ColliderComponent : BaseComponent
    {
        private bool mapCollision;
        public bool MapCollision => mapCollision;

        private Rectangle collider;
        public Rectangle Collider
        {
            get => collider;
            set => collider = value;
        }

        public ColliderComponent()
            : this(true)
        {
            
        }

        public ColliderComponent(bool mapCollision)
        {
            this.mapCollision = mapCollision;
        }
    }
}

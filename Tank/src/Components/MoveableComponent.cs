using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Components
{
    class MoveableComponent : BaseComponent
    {
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        private bool onGround;
        public bool OnGround
        {
            get => onGround;
            set => onGround = value;
        }

        public MoveableComponent()
        {
            allowMultiple = false;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Components
{
    /// <summary>
    /// Make a entity moveable
    /// </summary>
    class MoveableComponent : BaseComponent
    {
        /// <summary>
        /// The velocity of the entity
        /// </summary>
        private Vector2 velocity;

        /// <summary>
        /// Public access to the velocity
        /// </summary>
        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        /// <summary>
        /// Should be rotated by physic
        /// </summary>
        public bool PhysicRotate;

        /// <summary>
        /// Is the entity on the ground
        /// </summary>
        private bool onGround;

        /// <summary>
        /// Public access if the entity is on the ground
        /// </summary>
        public bool OnGround
        {
            get => onGround;
            set => onGround = value;
        }
    }
}

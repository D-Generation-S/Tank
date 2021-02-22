using Microsoft.Xna.Framework;

namespace Tank.Components
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
        /// The current acceleration
        /// </summary>
        private Vector2 acceleration;

        /// <summary>
        /// The current acceleration
        /// </summary>
        public Vector2 Acceleration
        {
            get => acceleration;
            set => acceleration = value;
        }

        /// <summary>
        /// The mass of the object
        /// </summary>
        private float mass;

        /// <summary>
        /// The private mass of the object
        /// </summary>
        public float Mass
        {
            get => mass;
            set => mass = value;
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

        public MoveableComponent()
        {
            mass = 1;
        }
    }
}

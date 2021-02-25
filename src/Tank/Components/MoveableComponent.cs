using Microsoft.Xna.Framework;

namespace Tank.Components
{
    /// <summary>
    /// Make a entity moveable
    /// </summary>
    class MoveableComponent : BaseComponent
    {
        /// <summary>
        /// Public access to the velocity
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The current acceleration
        /// </summary>
        public Vector2 Acceleration { get; set; }

        /// <summary>
        /// The private mass of the object
        /// </summary>
        public float Mass { get; set; }

        /// <summary>
        /// Should be rotated by physic
        /// </summary>
        public bool PhysicRotate { get; set; }

        /// <summary>
        /// Public access if the entity is on the ground
        /// </summary>
        public bool OnGround { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public MoveableComponent()
        {
            Priority = 50;
        }

        /// <inheritdoc/>
        public override void Init()
        {
            Mass = 1;
            OnGround = false;
            PhysicRotate = false;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }
    }
}

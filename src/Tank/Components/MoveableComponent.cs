using Microsoft.Xna.Framework;
using TankEngine.EntityComponentSystem.Components;

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
        /// Should physic be pplied to this entity
        /// </summary>
        public bool ApplyPhysic { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            Mass = 1;
            ApplyPhysic = false;
            PhysicRotate = false;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }
    }
}

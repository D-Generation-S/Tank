using Microsoft.Xna.Framework;

namespace Tank.Events.PhysicBased
{
    /// <summary>
    /// Apply force event class
    /// </summary>
    internal class ApplyForceEvent : EntityBasedEvent
    {
        /// <summary>
        /// The force to apply
        /// </summary>
        private readonly Vector2 force;

        /// <summary>
        /// The force to apply
        /// </summary>
        public Vector2 Force => force;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="entityId">The entity to apply the force on</param>
        /// <param name="force">The force to apply</param>
        public ApplyForceEvent(uint entityId, Vector2 force) : base(entityId)
        {
            this.force = force;
        }
    }
}

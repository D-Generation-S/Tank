using Microsoft.Xna.Framework;
using TankEngine.EntityComponentSystem.Events;

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
        public Vector2 Force;
    }
}

using Microsoft.Xna.Framework;

namespace Tank.Events.PhysicBased
{
    internal class ApplyForceEvent : EntityBasedEvent
    {

        private readonly Vector2 force;
        public Vector2 Force => force;

        public ApplyForceEvent(uint entityId, Vector2 force) : base(entityId)
        {
            this.force = force;
        }
    }
}

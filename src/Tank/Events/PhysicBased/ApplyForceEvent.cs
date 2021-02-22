using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.src.Events;

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

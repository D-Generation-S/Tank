using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.EntityBased
{
    /// <summary>
    /// A class instance will tell the system to remove an entity
    /// </summary>
    class RemoveEntityEvent : EntityBasedEvent
    {
        /// <summary>
        /// Create a new instance of this event
        /// </summary>
        /// <param name="entityId">The id of the event which got removed</param>
        public RemoveEntityEvent(uint entityId) : base(entityId)
        {
        }
    }
}

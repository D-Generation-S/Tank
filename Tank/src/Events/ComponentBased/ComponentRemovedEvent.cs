using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.ComponentBased
{
    /// <summary>
    /// This event will tell you that a component was remove from an entity
    /// </summary>
    class ComponentRemovedEvent : EntityBasedEvent
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="entityId">The id of the entity a component was removed from</param>
        public ComponentRemovedEvent(uint entityId) : base(entityId)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.ComponentBased
{
    /// <summary>
    /// This event will tell the systems that a new component was added
    /// </summary>
    class NewComponentEvent : EntityBasedEvent
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="entityId">The id of the entity where a component was added to</param>
        public NewComponentEvent(uint entityId) : base(entityId)
        {
        }
    }
}

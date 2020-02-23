using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.EntityBased
{
    /// <summary>
    /// This class event can be used to tell systems that there is a new entity
    /// </summary>
    class NewEntityEvent : EntityBasedEvent
    {
        /// <summary>
        /// Create a new instance of this event
        /// </summary>
        /// <param name="entityId">The id of the entity which was added</param>
        public NewEntityEvent(uint entityId) : base(entityId)
        {
        }
    }
}

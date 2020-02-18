using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.EntityBased
{
    /// <summary>
    /// This class will tell systems that an entity was removed from the manager
    /// </summary>
    class EntityRemovedEvent : EntityBasedEvent
    {
        /// <summary>
        /// Create a new instance of the class
        /// </summary>
        /// <param name="entityId">The id of the entity which was removed</param>
        public EntityRemovedEvent(uint entityId) : base(entityId)
        {
        }
    }
}

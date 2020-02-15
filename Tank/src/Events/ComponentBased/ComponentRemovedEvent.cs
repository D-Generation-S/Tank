using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.ComponentBased
{
    class ComponentRemovedEvent : EntityBasedEvent
    {
        public ComponentRemovedEvent(uint entityId) : base(entityId)
        {
        }
    }
}

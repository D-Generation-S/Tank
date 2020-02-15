using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.EntityBased
{
    class EntityRemovedEvent : EntityBasedEvent
    {
        public EntityRemovedEvent(uint entityId) : base(entityId)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.EntityBased
{
    class RemoveEntityEvent : EntityBasedEvent
    {
        public RemoveEntityEvent(uint entityId) : base(entityId)
        {
        }
    }
}

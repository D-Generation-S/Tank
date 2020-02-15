using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.ComponentBased
{
    class NewComponentEvent : EntityBasedEvent
    {
        public NewComponentEvent(uint entityId) : base(entityId)
        {
        }
    }
}

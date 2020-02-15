using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.EntityBased
{
    class NewEntityEvent : EntityBasedEvent
    {
        public NewEntityEvent(uint entityId) : base(entityId)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events
{
    class EntityBasedEvent : EventArgs
    {
        private readonly uint entityId;
        public uint EntityId => entityId;

        public EntityBasedEvent(uint entityId)
        {
            this.entityId = entityId;
        }
    }
}

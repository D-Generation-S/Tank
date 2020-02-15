using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.ComponentBased
{
    class RemoveComponentEvent : EntityBasedEvent
    {
        private readonly Type componentType;
        public Type ComponentType => componentType;

        public RemoveComponentEvent(uint entityId, Type componentType) : base(entityId)
        {
            this.componentType = componentType;
        }
    }
}

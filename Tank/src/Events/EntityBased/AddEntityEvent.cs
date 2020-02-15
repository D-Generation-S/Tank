using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.EntityBased
{
    class AddEntityEvent : EntityBasedEvent
    {
        private readonly List<IGameComponent> components;
        public List<IGameComponent> Components => components;

        public AddEntityEvent(uint entityId) : base(entityId)
        {
        }


        public AddEntityEvent(uint entityId, List<IGameComponent> components) : base(entityId)
        {
            this.components = components;
        }
    }
}

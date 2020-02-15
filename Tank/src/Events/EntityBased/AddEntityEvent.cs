using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.EntityComponentSystem;

namespace Tank.src.Events.EntityBased
{
    class AddEntityEvent : EventArgs
    {
        private readonly List<IComponent> components;
        public List<IComponent> Components => components;

        public AddEntityEvent(List<IComponent> components)
        {
            this.components = components;
        }
    }
}

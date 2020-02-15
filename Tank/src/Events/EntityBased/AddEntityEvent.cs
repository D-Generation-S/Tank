using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.EntityBased
{
    class AddEntityEvent : EventArgs
    {
        private readonly List<IGameComponent> components;
        public List<IGameComponent> Components => components;

        public AddEntityEvent(List<IGameComponent> components)
        {
            this.components = components;
        }
    }
}

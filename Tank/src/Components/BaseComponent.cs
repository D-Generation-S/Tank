using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.EntityComponentSystem;

namespace Tank.src.Components
{
    class BaseComponent : IGameComponent
    {
        private uint entityId;
        public uint EntityId => entityId;

        protected bool allowMultiple;
        public bool AllowMultiple => allowMultiple;

        public void SetEntityId(uint newId)
        {
            entityId = newId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.DataStructure
{
    class AsyncComponentRemoveContainer
    {
        public uint EntityId { get; set; }
        public Type ComponentType { get; set; }
    }
}

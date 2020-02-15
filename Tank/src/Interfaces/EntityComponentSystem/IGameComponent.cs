using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Interfaces.EntityComponentSystem
{
    interface IGameComponent
    {
        uint EntityId { get; }

        bool AllowMultiple { get; }

        void SetEntityId(uint newId);
    }
}

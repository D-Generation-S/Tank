using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.EntityComponentSystem;

namespace Tank.src.Interfaces.Factories
{
    interface IGameObjectFactory
    {
        List<IComponent> GetGameObjects();
    }
}

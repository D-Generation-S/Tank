using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.Container;
using Tank.Interfaces.Entity;

namespace Tank.Interfaces.Implementations
{
    interface ISystem : IEntity
    {
        void SetParent(ISystem parentSystem);

        void RecieveMessage(ISystemMessage message);

        string AddEntity(IEntity entity);

        bool RemoveEntity(string entityName);
    }
}

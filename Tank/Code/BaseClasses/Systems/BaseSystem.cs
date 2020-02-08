using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses.Entites;
using Tank.Interfaces.Container;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;

namespace Tank.Code.BaseClasses.Systems
{
    abstract class BaseSystem : BaseEntity, ISystem
    {
        protected ISystem parent;

        public virtual void SetParent(ISystem parentSystem)
        {
            parent = parentSystem;
        }

        public abstract string AddEntity(IEntity entity);
        public abstract bool RemoveEntity(string entityName);

        public virtual void RecieveMessage(ISystemMessage message)
        {
        }

        protected virtual void SendMessage(ISystemMessage message)
        {
            parent.RecieveMessage(message);
        }
    }
}

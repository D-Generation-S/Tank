using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.src.EntityComponentSystem.DataContainer
{
    class RecieverContainer
    {
        private readonly Type notificationType;
        public Type NotificationType => notificationType;

        private readonly List<IEventReceiver> receivers;
        public List<IEventReceiver> EventReceivers => receivers;

        /// <summary>
        /// Create a new container for receivers
        /// </summary>
        /// <param name="containerType">The type of the container to create</param>
        public RecieverContainer(Type containerType)
        {
            notificationType = containerType;
            receivers = new List<IEventReceiver>();
        }
    }
}

using System;
using System.Collections.Generic;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.EntityComponentSystem.DataContainer
{
    /// <summary>
    /// A container for a set of recievers assigned to an type
    /// </summary>
    class RecieverContainer
    {
        /// <summary>
        /// Readonly type of the notification this container is based on
        /// </summary>
        private readonly Type notificationType;

        /// <summary>
        /// Public readonly access to the notification type of this container
        /// </summary>
        public Type NotificationType => notificationType;

        /// <summary>
        /// A readonly list of all the receivers assigned to this event type
        /// </summary>
        private readonly List<IEventReceiver> receivers;

        /// <summary>
        /// A readonly access to all the assigned receivers
        /// </summary>
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

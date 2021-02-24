using System;
using System.Collections.Generic;
using Tank.EntityComponentSystem.DataContainer;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.EntityComponentSystem.Manager
{
    /// <summary>
    /// This class is the default event manager to use
    /// </summary>
    class EventManager : IEventManager
    {
        /// <summary>
        /// A list of the types and there recievers
        /// </summary>
        private readonly List<RecieverContainer> eventReceivers;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public EventManager()
        {
            eventReceivers = new List<RecieverContainer>();
        }

        /// <inheritdoc/>
        public void FireEvent<T>(object sender, T args) where T : EventArgs
        {
            Type type = typeof(T);
            RecieverContainer matchingContainer = eventReceivers.Find((container) =>
            {
                return container.NotificationType == type;
            });
            if (matchingContainer == null)
            {
                return;
            }
            foreach (IEventReceiver receiver in matchingContainer.EventReceivers)
            {
                if (receiver == sender)
                {
                    continue;
                }
                receiver.EventNotification(sender, args);
            }
        }

        /// <inheritdoc/>
        public void SubscribeEvent(IEventReceiver eventReciver, Type eventType)
        {
            RecieverContainer containerForSubscription = eventReceivers.Find((container) =>
            {
                return container.NotificationType == eventType;
            });
            if (containerForSubscription == null)
            {
                RecieverContainer container = new RecieverContainer(eventType);
                container.EventReceivers.Add(eventReciver);
                eventReceivers.Add(container);
                return;
            }
            containerForSubscription.EventReceivers.Add(eventReciver);
        }

        /// <inheritdoc/>
        public void UnsubscibeEvent(IEventReceiver eventReciver, Type eventType)
        {
            RecieverContainer containerToRemove = eventReceivers.Find((container) =>
            {
                return container.NotificationType == eventType;

            });
            if (containerToRemove != null)
            {
                containerToRemove.EventReceivers.Remove(eventReciver);
            }
        }

        /// <inheritdoc/>
        public void RemoveListner(IEventReceiver eventReciver)
        {
            eventReceivers.ForEach(item => item.EventReceivers.Remove(eventReciver));
        }

        /// <inheritdoc/>
        public void Clear()
        {
            eventReceivers.Clear();
        }
    }
}

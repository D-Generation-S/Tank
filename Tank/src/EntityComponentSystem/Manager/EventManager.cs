using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.EntityComponentSystem.DataContainer;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.src.EntityComponentSystem.Manager
{
    class EventManager : IEventManager
    {
        private readonly List<RecieverContainer> eventReceivers;

        public EventManager()
        {
            eventReceivers = new List<RecieverContainer>();
        }

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

        public void RemoveListner(IEventReceiver eventReciver)
        {
            throw new NotImplementedException();
        }

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
    }
}

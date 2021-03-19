using System;
using System.Collections.Generic;
using Tank.EntityComponentSystem.DataContainer;
using Tank.Events;
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
        /// A list with all the used events
        /// </summary>
        private readonly List<IGameEvent> usedEvents;

        /// <summary>
        /// The number of events to store in the pool
        /// </summary>
        private int maxEventsToStore;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public EventManager()
        {
            eventReceivers = new List<RecieverContainer>();
            usedEvents = new List<IGameEvent>();
            maxEventsToStore = 100;
        }

        /// <inheritdoc/>
        public void FireEvent<T>(object sender, T args) where T : IGameEvent
        {
            RecieverContainer matchingContainer = eventReceivers.Find((container) =>
            {
                return container.NotificationType == args.Type;
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
            StoreEvent(args);
        }

        /// <summary>
        /// Store a given event in the pool
        /// </summary>
        /// <param name="gameEvent">The game event to store</param>
        private void StoreEvent(IGameEvent gameEvent)
        {
            if (usedEvents.Count >= maxEventsToStore)
            {
                usedEvents.RemoveAt(0);
            }
            usedEvents.Add(gameEvent);
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

        /// <inheritdoc/>
        public T CreateEvent<T>() where T : IGameEvent
        {
            Type type = typeof(T);
            T returnEvent = (T)usedEvents.Find(eventData => eventData.Type == type);
            if (returnEvent != null)
            {
                usedEvents.Remove(returnEvent);
                returnEvent.Init();
            }
            return returnEvent == null ? (T)Activator.CreateInstance(type) : returnEvent;
        }
    }
}

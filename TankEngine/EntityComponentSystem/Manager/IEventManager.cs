using System;
using TankEngine.EntityComponentSystem.Events;

namespace TankEngine.EntityComponentSystem.Manager
{
    /// <summary>
    /// This interface is the core of the event manager and allows you to subscribe, unsubscribe and fire events
    /// </summary>
    public interface IEventManager : IClearable
    {
        /// <summary>
        /// Create a event of a given type
        /// </summary>
        /// <typeparam name="T">The type of the event</typeparam>
        /// <returns>An instance of the requested event</returns>
        T CreateEvent<T>() where T : IGameEvent;

        /// <summary>
        /// This method will fire an event for the whole system
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void FireEvent<T>(object sender, T args) where T : IGameEvent;

        /// <summary>
        /// This method will allow you to add an new listner to the event manager
        /// </summary>
        /// <param name="eventReceiver">The listner to remove</param>
        /// <param name="eventType">The type of event to subscibe to</param>
        void SubscribeEvent(IEventReceiver eventReceiver, Type eventType);

        /// <summary>
        /// This method will allow you to stop listning to an event
        /// </summary>
        /// <param name="eventReceiver">The listner to remove</param>
        /// <param name="eventType">The type of event to unsubcribe from</param>
        void UnsubscibeEvent(IEventReceiver eventReceiver, Type eventType);

        /// <summary>
        /// This method will allow you to remove an event listner
        /// </summary>
        /// <param name="eventReceiver">The listner to remove</param>
        void RemoveListner(IEventReceiver eventReceiver);
    }
}

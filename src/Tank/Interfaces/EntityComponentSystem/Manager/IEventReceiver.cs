using System;

namespace Tank.Interfaces.EntityComponentSystem.Manager
{
    /// <summary>
    /// This interface is part of the event system and allows a class to recieve events after subscribing
    /// </summary>
    interface IEventReceiver
    {
        /// <summary>
        /// This method is called if the reciver should be informed that an event got triggered.
        /// </summary>
        /// <param name="eventArgs">The arguments of the fired event</param>
        void EventNotification(object sender, EventArgs eventArgs);
    }
}

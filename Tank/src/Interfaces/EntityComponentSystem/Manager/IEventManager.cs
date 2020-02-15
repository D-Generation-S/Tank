﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Interfaces.EntityComponentSystem.Manager
{
    interface IEventManager
    {
        /// <summary>
        /// This method will fire an event for the whole system
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void FireEvent<T>(object sender, T args) where T : EventArgs;

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
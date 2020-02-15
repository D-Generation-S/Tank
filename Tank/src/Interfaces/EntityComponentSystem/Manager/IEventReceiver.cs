using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Interfaces.EntityComponentSystem.Manager
{
    interface IEventReceiver
    {
        /// <summary>
        /// This method is called if the reciver should be informed that an event got triggered.
        /// </summary>
        /// <param name="eventArgs">The arguments of the fired event</param>
        void EventNotification(object sender, EventArgs eventArgs);
    }
}

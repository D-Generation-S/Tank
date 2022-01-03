using System;

namespace Tank.Events
{
    /// <summary>
    /// Base class for every event
    /// </summary>
    public class BaseEvent : IGameEvent
    {
        /// <summary>
        /// The type of the event
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Create a new instance of this event
        /// </summary>
        public BaseEvent()
        {
            Type = GetType();
        }

        /// <inheritdoc/>
        public virtual void Init()
        {
        }
    }
}

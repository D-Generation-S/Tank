using System;

namespace Tank.Events
{
    /// <summary>
    /// Base event type
    /// </summary>
    public interface IGameEvent
    {
        /// <summary>
        /// The type of the event
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Reset this event so it can be reused
        /// </summary>
        void Init();
    }
}

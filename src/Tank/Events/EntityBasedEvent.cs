using System;

namespace Tank.Events
{
    /// <summary>
    /// This class is a base class to defined entity based events
    /// </summary>
    class EntityBasedEvent : EventArgs
    {
        /// <summary>
        /// The id of the entity the event is related to
        /// </summary>
        private readonly uint entityId;

        /// <summary>
        /// Readonly access to the id of the entity the events is related to
        /// </summary>
        public uint EntityId => entityId;

        /// <summary>
        /// Construct a new instance of this class
        /// </summary>
        /// <param name="entityId">The id of the entity the event belongs to</param>
        public EntityBasedEvent(uint entityId)
        {
            this.entityId = entityId;
        }
    }
}

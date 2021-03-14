using System;

namespace Tank.Events
{
    /// <summary>
    /// This class is a base class to defined entity based events
    /// </summary>
    class EntityBasedEvent : IGameEvent
    {
        /// <summary>
        /// Readonly access to the id of the entity the events is related to
        /// </summary>
        public uint EntityId;

        public Type Type{ get; }

        /// <summary>
        /// Construct a new instance of this class
        /// </summary>
        /// <param name="entityId">The id of the entity the event belongs to</param>
        public EntityBasedEvent()
        {
            Type = this.GetType();
        }

        public virtual void Init()
        {
            EntityId = 0;
        }
    }
}

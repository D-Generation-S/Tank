namespace Tank.Events.ComponentBased
{
    /// <summary>
    /// This event will tell you that a component was remove from an entity
    /// </summary>
    class ComponentRemovedEvent : EntityBasedEvent
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="entityId">The id of the entity a component was removed from</param>
        public ComponentRemovedEvent(uint entityId) : base(entityId)
        {
        }
    }
}

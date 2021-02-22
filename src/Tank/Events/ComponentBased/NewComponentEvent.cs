namespace Tank.Events.ComponentBased
{
    /// <summary>
    /// This event will tell the systems that a new component was added
    /// </summary>
    class NewComponentEvent : EntityBasedEvent
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="entityId">The id of the entity where a component was added to</param>
        public NewComponentEvent(uint entityId) : base(entityId)
        {
        }
    }
}

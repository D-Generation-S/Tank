namespace Tank.Events.EntityBased
{
    /// <summary>
    /// This class event can be used to tell systems that there is a new entity
    /// </summary>
    class NewEntityEvent : EntityBasedEvent
    {
        /// <summary>
        /// Create a new instance of this event
        /// </summary>
        /// <param name="entityId">The id of the entity which was added</param>
        public NewEntityEvent(uint entityId) : base(entityId)
        {
        }
    }
}

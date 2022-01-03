namespace Tank.Events
{
    /// <summary>
    /// This class is a base class to defined entity based events
    /// </summary>
    public class EntityBasedEvent : BaseEvent
    {
        /// <summary>
        /// Readonly access to the id of the entity the events is related to
        /// </summary>
        public uint EntityId;

        /// <inheritdoc/>
        public override void Init()
        {
            EntityId = 0;
        }
    }
}

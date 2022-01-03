using System.Collections.Generic;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.Events.EntityBased
{
    /// <summary>
    /// This class can be used to add a new entity to the manager
    /// </summary>
    public class AddEntityEvent : BaseEvent
    {

        /// <summary>
        /// Readonly acces to the components which should be added to the entity
        /// </summary>
        public List<IComponent> Components;

        /// <inheritdoc/>
        public override void Init()
        {
            Components.Clear();
        }
    }
}

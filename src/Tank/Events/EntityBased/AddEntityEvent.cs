using System;
using System.Collections.Generic;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.Events.EntityBased
{
    /// <summary>
    /// This class can be used to add a new entity to the manager
    /// </summary>
    class AddEntityEvent : IGameEvent
    {

        /// <summary>
        /// Readonly acces to the components which should be added to the entity
        /// </summary>
        public List<IComponent> Components;

        public Type Type { get; }

        public AddEntityEvent()
        {
            Type = this.GetType();
        }

        public void Init()
        {
            Components.Clear();
        }
    }
}

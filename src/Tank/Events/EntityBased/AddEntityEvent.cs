using System;
using System.Collections.Generic;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.Events.EntityBased
{
    /// <summary>
    /// This class can be used to add a new entity to the manager
    /// </summary>
    class AddEntityEvent : EventArgs
    {
        /// <summary>
        /// A list with all the components which should be added
        /// </summary>
        private readonly List<IComponent> components;

        /// <summary>
        /// Readonly acces to the components which should be added to the entity
        /// </summary>
        public List<IComponent> Components => components;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="components">All the components to add to the entity</param>
        public AddEntityEvent(List<IComponent> components)
        {
            this.components = components;
        }
    }
}

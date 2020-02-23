using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Events.ComponentBased
{
    /// <summary>
    /// This event will allow you to remove an component from an entity
    /// </summary>
    class RemoveComponentEvent : EntityBasedEvent
    {
        /// <summary>
        /// The type of the component to remove
        /// </summary>
        private readonly Type componentType;

        /// <summary>
        /// Readonly access to the component to remove
        /// </summary>
        public Type ComponentType => componentType;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="entityId">The id of the entity to remove</param>
        /// <param name="componentType">The component type to remove</param>
        public RemoveComponentEvent(uint entityId, Type componentType) : base(entityId)
        {
            this.componentType = componentType;
        }
    }
}

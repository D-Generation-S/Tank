using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.EntityComponentSystem;

namespace Tank.Components
{
    /// <summary>
    /// This class represents a component base class making the implementation easier
    /// </summary>
    class BaseComponent : IComponent
    {
        /// <summary>
        /// The id of the entity the component is assigned to
        /// </summary>
        private uint entityId;

        /// <summary>
        /// Readonly access to the entity id the component is assigned to
        /// </summary>
        public uint EntityId => entityId;

        /// <summary>
        /// Allow multiple instances of this component per entity
        /// </summary>
        protected bool allowMultiple;

        /// <summary>
        /// Readonly access if multiple component instances are allowed per entity
        /// </summary>
        public bool AllowMultiple => allowMultiple;

        /// <inheritdoc/>
        public void SetEntityId(uint newId)
        {
            entityId = newId;
        }
    }
}

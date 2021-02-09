﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Interfaces.EntityComponentSystem
{
    /// <summary>
    /// This interface is representing a component from an entity
    /// </summary>
    interface IComponent
    {
        /// <summary>
        /// The id of the entity this component belongs to
        /// </summary>
        uint EntityId { get; }

        /// <summary>
        /// Is it allowed to add mulitple instances from this component to an entity
        /// </summary>
        bool AllowMultiple { get; }

        /// <summary>
        /// Set the id of the entity this component is assigned to
        /// </summary>
        /// <param name="newId">The new id for this component</param>
        void SetEntityId(uint newId);
    }
}
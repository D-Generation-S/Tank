﻿using System;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.Components
{
    /// <summary>
    /// This class represents a component base class making the implementation easier
    /// </summary>
    abstract class BaseComponent : IComponent
    {
        /// <inheritdoc/>
        public uint EntityId { get; private set; }

        /// <inheritdoc/>
        public bool AllowMultiple { get; }

        /// <inheritdoc/>
        public Type Type { get; }

        /// <inheritdoc/>
        public int Priority { get; protected set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public BaseComponent()
        {
            AllowMultiple = false;
            Type = GetType();
            Priority = 0;
            Init();
        }

        /// <inheritdoc/>
        public void SetEntityId(uint newId)
        {
            EntityId = newId;
        }

        /// <inheritdoc/>
        public abstract void Init();
    }
}

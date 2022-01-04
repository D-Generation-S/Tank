using System;

namespace TankEngine.EntityComponentSystem.Components
{
    /// <summary>
    /// This class represents a component base class making the implementation easier
    /// </summary>
    public abstract class BaseComponent : IComponent
    {
        /// <inheritdoc/>
        public uint EntityId { get; private set; }

        /// <inheritdoc/>
        public bool AllowMultiple { get; }

        /// <inheritdoc/>
        public Type Type { get; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public BaseComponent()
        {
            AllowMultiple = false;
            Type = GetType();
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

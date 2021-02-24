using System;

namespace Tank.DataStructure
{
    /// <summary>
    /// This structure will store informations for async removable of components
    /// </summary>
    class AsyncComponentRemoveContainer
    {
        /// <summary>
        /// The entity id to remove the components from
        /// </summary>
        public uint EntityId { get; set; }

        /// <summary>
        /// The component type to remove
        /// </summary>
        public Type ComponentType { get; set; }
    }
}

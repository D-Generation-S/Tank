using System.Collections.Generic;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.src.Interfaces.Factories
{
    /// <summary>
    /// The interface describes a factory for creating components based on a defined ruleset
    /// </summary>
    interface IGameObjectFactory
    {
        /// <summary>
        /// Get all the components from a new object
        /// </summary>
        /// <returns></returns>
        List<IComponent> GetGameObjects();
    }
}

using System.Collections.Generic;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.Interfaces.Builders
{
    /// <summary>
    /// This interface defines a builder to create a component list
    /// </summary>
    interface IGameObjectBuilder
    {
        /// <summary>
        /// The method to call to build the game components for the new entity
        /// </summary>
        /// <returns>A list with all the components neede for the new game object</returns>
        List<IComponent> BuildGameComponents();
    }
}

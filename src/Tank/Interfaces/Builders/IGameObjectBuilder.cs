using System.Collections.Generic;
using Tank.EntityComponentSystem.Manager;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;

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

        /// <summary>
        /// Initialize this builder
        /// </summary>
        /// <param name="entityManager">The entity manager to use</param>
        void Init(IEntityManager entityManager);

        /// <summary>
        /// Initialize this builder
        /// </summary>
        /// <param name="engine">The engine to use</param>
        void Init(IGameEngine engine);
    }
}

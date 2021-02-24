using System.Collections.Generic;
using Tank.Interfaces.Builders;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Builders
{
    /// <summary>
    /// Base builder class
    /// </summary>
    abstract class BaseBuilder : IGameObjectBuilder
    {
        /// <summary>
        /// The entity manager to use
        /// </summary>
        protected IEntityManager entityManager;

        /// <inheritdoc/>
        public abstract List<IComponent> BuildGameComponents();

        /// <inheritdoc/>
        public void Init(IEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        /// <inheritdoc/>
        public void Init(IGameEngine engine)
        {
            Init(engine.EntityManager);
        }
    }
}

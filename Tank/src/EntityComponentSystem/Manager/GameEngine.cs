using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.EntityComponentSystem;
using Tank.src.Interfaces.EntityComponentSystem.Manager;
using Tank.src.Wrapper;

namespace Tank.src.EntityComponentSystem.Manager
{
    /// <summary>
    /// This class is a container for all the game engine components
    /// </summary>
    class GameEngine : IGameEngine
    {
        /// <summary>
        /// A instance of the event manager to use in the current game engine
        /// </summary>
        private readonly IEventManager eventManager;

        /// <summary>
        /// Readonly access to the event manager for classes from the outside
        /// </summary>
        public IEventManager EventManager => eventManager;

        /// <summary>
        /// A instance of the entity manager to use in the current game engine
        /// </summary>
        private readonly IEntityManager entityManager;

        /// <summary>
        /// Readonly access to the entity manager for external reading
        /// </summary>
        public IEntityManager EntityManager => entityManager;

        /// <summary>
        /// A warpper class to provide the game engine some content
        /// </summary>
        private readonly ContentWrapper contentManager;

        /// <summary>
        /// Readonly access to the content manager for loading new content into the game
        /// </summary>
        public ContentWrapper ContentManager => contentManager;

        /// <summary>
        /// A private list of all the systems currently registered
        /// </summary>
        private readonly List<ISystem> systems;

        /// <summary>
        /// Create a new instance of the game engine
        /// </summary>
        /// <param name="eventManager">The event manager to use</param>
        /// <param name="entityManager">The entity manager to use</param>
        /// <param name="contentWrapper">The content warpper to use</param>
        public GameEngine(IEventManager eventManager, IEntityManager entityManager, ContentWrapper contentWrapper)
        {
            systems = new List<ISystem>();
            this.eventManager = eventManager;
            entityManager.Initialize(eventManager);
            this.entityManager = entityManager;
            contentManager = contentWrapper;
        }

        /// <inheritdoc/>
        public void AddSystem(ISystem systemToAdd)
        {
            systemToAdd.Initialize(this);
            systems.Add(systemToAdd);
        }

        /// <inheritdoc/>
        public void Update(GameTime gameTime)
        {
            foreach (ISystem system in systems)
            {
                system.Update(gameTime);
            }
        }

        /// <inheritdoc/>
        public void Draw(GameTime gameTime)
        {
            foreach (ISystem system in systems)
            {
                system.Draw(gameTime);
            }
        }
    }
}

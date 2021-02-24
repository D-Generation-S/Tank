using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Wrapper;

namespace Tank.EntityComponentSystem.Manager
{
    /// <summary>
    /// This class is a container for all the game engine components
    /// </summary>
    class GameEngine : IGameEngine
    {
        /// <summary>
        /// Is the engine currently locked
        /// </summary>
        private bool locked;

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
        /// A private list with all the new systems to add
        /// </summary>
        private readonly List<ISystem> systemsToAdd;

        /// <summary>
        /// A private list with all the systems to remove
        /// </summary>
        private readonly List<ISystem> systemsToRemove;

        /// <summary>
        /// Create a new instance of the game engine
        /// </summary>
        /// <param name="eventManager">The event manager to use</param>
        /// <param name="entityManager">The entity manager to use</param>
        /// <param name="contentWrapper">The content warpper to use</param>
        public GameEngine(IEventManager eventManager, IEntityManager entityManager, ContentWrapper contentWrapper)
        {
            systems = new List<ISystem>();
            systemsToAdd = new List<ISystem>();
            systemsToRemove = new List<ISystem>();
            this.eventManager = eventManager;
            entityManager.Initialize(eventManager);
            this.entityManager = entityManager;
            contentManager = contentWrapper;
        }

        /// <inheritdoc/>
        public void AddSystem(ISystem systemToAdd)
        {
            systemToAdd.Initialize(this);
            systemsToAdd.Add(systemToAdd);
        }

        /// <inheritdoc/>
        public void AddSystems(params ISystem[] systemsToAdd)
        {
            foreach (ISystem system in systemsToAdd)
            {
                AddSystem(system);
            }
        }

        /// <inheritdoc/>
        public void Update(GameTime gameTime)
        {
            if (!locked)
            {
                for (int i = systemsToAdd.Count - 1; i >= 0; i--)
                {
                    systems.Add(systemsToAdd[i]);
                    systemsToAdd.RemoveAt(i);
                }

                for (int i = systemsToRemove.Count - 1; i >= 0; i--)
                {
                    systems.Remove(systemsToRemove[i]);
                    systemsToRemove.RemoveAt(i);
                }
            }

            foreach (ISystem system in systems)
            {
                system.Update(gameTime);
            }
        }

        /// <inheritdoc/>
        public void Draw(GameTime gameTime)
        {
            locked = true;
            foreach (ISystem system in systems)
            {
                system.Draw(gameTime);
            }
            locked = false;
        }

        /// <inheritdoc/>
        public int GetEntityCount()
        {
            return entityManager.GetEntityCount();
        }

        /// <inheritdoc/>
        public int GetComponentCount()
        {
             return entityManager.GetComponentCount();
        }

        /// <inheritdoc/>
        public int GetSystemCount()
        {
            return systems.Count;
        }

        public int GetUsedComponentCount()
        {
            return entityManager.GetUsedComponentCount();
        }
    }
}

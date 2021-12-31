using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;
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
        /// The next system id
        /// </summary>
        private uint nextSystemId;

        /// <summary>
        /// The time from the last call
        /// </summary>
        private float previousTime;

        /// <summary>
        /// The time left over from the last physic calculation
        /// </summary>
        private float leftOverDeltaTime;

        /// <summary>
        /// Was the engine restored lately
        /// </summary>
        private bool gotRestored;

        /// <summary>
        /// A number of milliseconds each physic update is allowed to use, is more was used in the last call it will be called twice in this call
        /// </summary>
        private readonly float physicMillisecondsDeltaTime;

        /// <summary>
        /// Readonly access to the event manager for classes from the outside
        /// </summary>
        public IEventManager EventManager { get; }

        /// <summary>
        /// Readonly access to the entity manager for external reading
        /// </summary>
        public IEntityManager EntityManager { get; }

        /// <summary>
        /// Readonly access to the content manager for loading new content into the game
        /// </summary>
        public ContentWrapper ContentManager { get; }

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
            : this(eventManager, entityManager, contentWrapper, 60)
        {

        }

        public GameEngine(IEventManager eventManager, IEntityManager entityManager, ContentWrapper contentWrapper, int physicUpdatesPerSecond)
        {
            systems = new List<ISystem>();
            systemsToAdd = new List<ISystem>();
            systemsToRemove = new List<ISystem>();
            EventManager = eventManager;
            entityManager.Initialize(eventManager);
            EntityManager = entityManager;
            ContentManager = contentWrapper;
            nextSystemId = 0;
            gotRestored = true;
            physicMillisecondsDeltaTime = 1000 / physicUpdatesPerSecond;
        }

        /// <inheritdoc/>
        public void AddSystem(ISystem systemToAdd)
        {
            systemToAdd.Initialize(this);
            systemToAdd.SetSystemId(nextSystemId);
            systemsToAdd.Add(systemToAdd);
            nextSystemId++;
        }

        public void RemoveSystem(ISystem systemToRemove)
        {
            systems.RemoveAll(system => system.SystemId == systemToRemove.SystemId);
        }

        /// <inheritdoc/>
        public void Update(GameTime gameTime)
        {
            float deltaTime = gameTime.TotalGameTime.Milliseconds - previousTime;
            previousTime = gameTime.TotalGameTime.Milliseconds;
            int timeSteps = (int)((deltaTime + leftOverDeltaTime) / physicMillisecondsDeltaTime);
            leftOverDeltaTime = deltaTime - (timeSteps * physicMillisecondsDeltaTime);
            if (gotRestored)
            {
                gotRestored = false;
                leftOverDeltaTime = 0;
                timeSteps = 1;
            }

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
                system.PreUpdate();

                for (int iteration = 0; iteration < timeSteps; iteration++)
                {
                    system.PhysicUpdate(gameTime);
                }
                system.Update(gameTime);
                system.LateUpdate();
            }

            EntityManager.LateUpdate();
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
            return EntityManager.GetEntityCount();
        }

        /// <inheritdoc/>
        public int GetComponentCount()
        {
            return EntityManager.GetComponentCount();
        }

        /// <inheritdoc/>
        public int GetSystemCount()
        {
            return systems.Count;
        }

        /// <inheritdoc/>
        public int GetUsedComponentCount()
        {
            return EntityManager.GetUsedComponentCount();
        }

        /// <inheritdoc/>
        public void Suspend()
        {
            foreach (ISystem system in systems)
            {
                system.Suspend();
            }
        }

        /// <inheritdoc/>
        public void Restore()
        {
            foreach (ISystem system in systems)
            {
                system.Restore();
            }
            gotRestored = true;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            locked = true;
            systemsToAdd.Clear();
            EventManager.Clear();
            for (int i = systems.Count - 1; i >= 0; i--)
            {
                RemoveSystem(systems[i]);
            }
            nextSystemId = 0;
            EntityManager.Clear();
            locked = false;
        }

        /// <inheritdoc/>
        public string GetSystemWatchedEntites()
        {
            StringBuilder stringBuilder = new StringBuilder("\n");
            foreach (ISystem system in systems)
            {
                stringBuilder.Append(system.GetType().Name);
                stringBuilder.Append(": ");
                stringBuilder.Append(system.WatchedEntitiesCount);
                stringBuilder.Append("\n");
            }
            return stringBuilder.ToString();
        }
    }
}

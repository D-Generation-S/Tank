using Microsoft.Xna.Framework;
using Tank.Wrapper;

namespace Tank.Interfaces.EntityComponentSystem.Manager
{
    /// <summary>
    /// This interface represents the game engine itself
    /// </summary>
    interface IGameEngine
    {
        /// <summary>
        /// The event manager to use in the game engine
        /// </summary>
        IEventManager EventManager { get; }

        /// <summary>
        /// The entity manager to use in the game engine
        /// </summary>
        IEntityManager EntityManager { get; }

        /// <summary>
        /// In instance of the content wrapper to load or get content from cache or loading it
        /// </summary>
        ContentWrapper ContentManager { get; }

        /// <summary>
        /// This method allows you to add a new system
        /// </summary>
        /// <param name="systemToAdd">The current system to add</param>
        void AddSystem(ISystem systemToAdd);

        /// <summary>
        /// This method allows you to add a undefined number of systems
        /// </summary>
        /// <param name="systemsToAdd">All the systems to add</param>
        void AddSystems(params ISystem[] systemsToAdd);

        /// <summary>
        /// Update this system
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draw this system
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        void Draw(GameTime gameTime);

        /// <summary>
        /// Get the total number of entities
        /// </summary>
        /// <returns>The total number of entities</returns>
        int GetEntityCount();

        /// <summary>
        /// The number of components
        /// </summary>
        /// <returns>The number of component</returns>
        int GetComponentCount();


        /// <summary>
        /// The number of used components
        /// </summary>
        /// <returns>The number of used components</returns>
        int GetUsedComponentCount();

        /// <summary>
        /// Get the number of systems
        /// </summary>
        /// <returns>The number of active systems</returns>
        int GetSystemCount();
    }
}

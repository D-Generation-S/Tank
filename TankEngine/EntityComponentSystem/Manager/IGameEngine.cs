using Microsoft.Xna.Framework;
using Tank.GameStates;
using TankEngine.Wrapper;

namespace Tank.Interfaces.EntityComponentSystem.Manager
{
    /// <summary>
    /// This interface represents the game engine itself
    /// </summary>
    public interface IGameEngine : IClearable, IRestoreable
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
        /// Remove a system from the system list
        /// </summary>
        void RemoveSystem(ISystem systemToRemove);

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

        /// <summary>
        /// Get a debug string for all the watched entites per system
        /// </summary>
        /// <returns>The entity count per system</returns>
        string GetSystemWatchedEntites();
    }
}

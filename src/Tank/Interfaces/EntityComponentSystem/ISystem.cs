using Microsoft.Xna.Framework;
using Tank.GameStates;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Interfaces.EntityComponentSystem
{
    /// <summary>
    /// This interface describes a systems which can be added to the entity manager
    /// </summary>
    interface ISystem : IEventReceiver, IRestoreable
    {
        /// <summary>
        /// All the watched entities in the system
        /// </summary>
        public int WatchedEntitiesCount { get; }

        /// <summary>
        /// The system id
        /// </summary>
        uint SystemId { get; }

        /// <summary>
        /// Set the system id for identification
        /// </summary>
        /// <param name="systemId">The system id to add</param>
        void SetSystemId(uint systemId);

        /// <summary>
        /// Initialize this system
        /// </summary>
        /// <param name="gameEngine">The current game engine</param>
        void Initialize(IGameEngine gameEngine);

        /// <summary>
        /// Called before the real update
        /// </summary>
        void PreUpdate();

        /// <summary>
        /// Update this system
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Update this system with a physic based update
        /// </summary>
        /// <param name="gameTime">The current Game Time</param>
        void PhysicUpdate(GameTime gameTime);

        /// <summary>
        /// Called after the update
        /// </summary>
        void LateUpdate();

        /// <summary>
        /// Draw this system
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        void Draw(GameTime gameTime);
    }
}

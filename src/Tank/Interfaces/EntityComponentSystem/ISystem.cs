using Microsoft.Xna.Framework;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Interfaces.EntityComponentSystem
{
    /// <summary>
    /// This interface describes a systems which can be added to the entity manager
    /// </summary>
    interface ISystem : IEventReceiver
    {
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
        /// Suspend this system
        /// </summary>
        void Suspend();

        /// <summary>
        /// Restore this system
        /// </summary>
        void Restore();
    }
}

using Tank.Interfaces.GameObjectControlling;

namespace Tank.Components
{
    /// <summary>
    /// This object is controlable by a player
    /// </summary>
    class PlayerControllableComponent : ControllableGameObject
    {
        /// <summary>
        /// The controller to use
        /// </summary>
        public IGameObjectController Controller { get; set; }

    }
}

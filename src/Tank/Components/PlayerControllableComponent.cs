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
        private readonly IGameObjectController controller;

        /// <summary>
        /// The controller to use
        /// </summary>
        public IGameObjectController Controller => controller;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="objectController">The controller to use</param>
        public PlayerControllableComponent(IGameObjectController objectController)
        {
            controller = objectController;
        }
    }
}

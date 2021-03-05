using Tank.GameStates;

namespace Tank.Commands.GameManager
{
    /// <summary>
    /// Abstract game manager command
    /// </summary>
    abstract class AbstractGameManagerCommand : ICommand
    {
        protected readonly GameStateManager gameStateManager;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="gameStateManager">The game state manager to use</param>
        public AbstractGameManagerCommand(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
        }

        /// <inheritdoc/>
        public virtual bool CanExecute()
        {
            return gameStateManager != null;
        }

        public abstract void Execute();
    }
}

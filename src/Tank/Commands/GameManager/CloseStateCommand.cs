using TankEngine.GameStates;

namespace Tank.Commands.GameManager
{
    /// <summary>
    /// Close a given state command
    /// </summary>
    class CloseStateCommand : AbstractGameManagerCommand
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="gameStateManager">The game state manager to use</param>
        public CloseStateCommand(GameStateManager gameStateManager) : base(gameStateManager)
        {
        }

        /// <inheritdoc/>
        public override void Execute()
        {
            if (!CanExecute())
            {
                return;
            }
            gameStateManager.Pop();
        }
    }
}

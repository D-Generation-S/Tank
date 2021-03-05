using Tank.GameStates;

namespace Tank.Commands.GameManager
{
    class CloseGameCommand : AbstractGameManagerCommand
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="gameStateManager">The state manager to use</param>
        public CloseGameCommand(GameStateManager gameStateManager) : base(gameStateManager)
        {
        }

        /// <inheritdoc/>
        public override void Execute()
        {
            gameStateManager.Clear();
        }
    }
}

using Tank.GameStates.States;
using TankEngine.GameStates;

namespace Tank.Commands.GameManager
{
    /// <summary>
    /// This class will remove all the states and return to the main menu
    /// </summary>
    class RevertToMainMenuCommand : AbstractGameManagerCommand
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="gameStateManager">The game state manager to use</param>
        public RevertToMainMenuCommand(GameStateManager gameStateManager) : base(gameStateManager)
        {
        }

        /// <inheritdoc/>
        public override void Execute()
        {
            gameStateManager.ResetState(new MainMenuState());
        }
    }
}

using TankEngine.GameStates;
using TankEngine.GameStates.States;

namespace Tank.Commands.GameManager
{
    /// <summary>
    /// This class will replace a state
    /// </summary>
    class ReplaceStateCommand : OpenAdditionalStateCommand
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="gameStateManager">The game state manager to use</param>
        /// <param name="stateToOpen">The state to use for replacement</param>
        public ReplaceStateCommand(GameStateManager gameStateManager, IState stateToOpen) : base(gameStateManager, stateToOpen)
        {
        }

        /// <inheritdoc/>
        public override void Execute()
        {
            if (!CanExecute())
            {
                return;
            }
            gameStateManager.Replace(stateToOpen);
        }
    }
}

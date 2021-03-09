using Tank.GameStates;
using Tank.GameStates.States;

namespace Tank.Commands.GameManager
{
    /// <summary>
    /// This class will open an additionam state
    /// </summary>
    class OpenAdditionalStateCommand : AbstractGameManagerCommand
    {
        /// <summary>
        /// The new state to open
        /// </summary>
        protected readonly IState stateToOpen;

        /// <summary>
        /// Suspend any acitve game state
        /// </summary>
        private readonly bool suspendExistingState;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="gameStateManager">The game state manager to use</param>
        /// <param name="stateToOpen">The state to open</param>
        public OpenAdditionalStateCommand(GameStateManager gameStateManager, IState stateToOpen)
            : this(gameStateManager, stateToOpen, true)
        {
            this.stateToOpen = stateToOpen;
        }


        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="gameStateManager">The game state manager to use</param>
        /// <param name="stateToOpen">The state to open</param>
        public OpenAdditionalStateCommand(GameStateManager gameStateManager, IState stateToOpen, bool suspendExistingState) : base(gameStateManager)
        {
            this.stateToOpen = stateToOpen;
            this.suspendExistingState = suspendExistingState;
        }

        /// <inheritdoc/>
        public override bool CanExecute()
        {
            return base.CanExecute() && stateToOpen != null;
        }

        /// <inheritdoc/>
        public override void Execute()
        {
            if (!CanExecute())
            {
                return;
            }
            gameStateManager.Add(stateToOpen, suspendExistingState);
        }
    }
}

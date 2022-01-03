using System;
using Tank.Commands;

namespace TankEngine.Commands
{
    /// <summary>
    /// Command to execute a action
    /// </summary>
    internal class ActionCommand : ICommand
    {
        /// <summary>
        /// The command to execute
        /// </summary>
        private readonly Action commandAction;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="commandAction">The command action to perform</param>
        public ActionCommand(Action commandAction)
        {
            this.commandAction = commandAction;
        }

        /// <inheritdoc>
        public bool CanExecute()
        {
            return commandAction != null;
        }

        /// <inheritdoc>
        public void Execute()
        {
            commandAction();
        }
    }
}

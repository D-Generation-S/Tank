namespace Tank.Commands
{
    /// <summary>
    /// Command interface
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// Can the command run with the current conditions
        /// </summary>
        /// <returns>True if it can run</returns>
        bool CanExecute();

        /// <summary>
        /// Execute the command
        /// </summary>
        void Execute();
    }
}

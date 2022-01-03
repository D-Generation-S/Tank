namespace Tank.GameStates
{
    /// <summary>
    /// Interface to define classes which are suspendable and restoreable
    /// </summary>
    public interface IRestoreable
    {
        /// <summary>
        /// Restore this state
        /// </summary>
        void Restore();

        /// <summary>
        /// Suspend this state
        /// </summary>
        void Suspend();
    }
}

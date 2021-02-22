namespace Tank.Interfaces.GameObjectControlling
{
    /// <summary>
    /// This interface will hide the used input device and generalize the input
    /// </summary>
    interface IGameObjectController
    {
        /// <summary>
        /// Update the state of this controller
        /// </summary>
        void UpdateStates();

        /// <summary>
        /// Is the fire key pressed
        /// </summary>
        /// <returns>True if the key was pressed once</returns>
        bool IsFirePressed();

        /// <summary>
        /// Should we increase the strength
        /// </summary>
        /// <returns>True if the strength should be increased</returns>
        bool IncreaseStrength();

        /// <summary>
        /// Should we decrease the strength
        /// </summary>
        /// <returns>True if we want to decrease the strength</returns>
        bool DecreseStrength();

        /// <summary>
        /// Should we rotate the barrel up
        /// </summary>
        /// <returns>true if we should rotate the barrel up</returns>
        bool RotateUp();

        /// <summary>
        /// Should we rotate the barrel down
        /// </summary>
        /// <returns>true if we should rotate the barrel down</returns>
        bool RotateDown();
    }
}

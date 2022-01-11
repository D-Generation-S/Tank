namespace TankEngine.EntityComponentSystem
{
    /// <summary>
    /// This game object can be cleared
    /// </summary>
    public interface IClearable
    {
        /// <summary>
        /// Clear the event manager
        /// </summary>
        void Clear();
    }
}

namespace TankEngine.DataStructures.Pools
{
    /// <summary>
    /// Interface for an object pool
    /// </summary>
    /// <typeparam name="T">The type of objects in the pool</typeparam>
    public interface IObjectPool<T>
    {
        /// <summary>
        /// Get a new object from the pool or create a new one
        /// </summary>
        /// <returns>A new objects of type T</returns>
        T Get();

        /// <summary>
        /// Return an item to the pool
        /// </summary>
        /// <param name="item">The item to return</param>
        void Return(T item);
    }
}
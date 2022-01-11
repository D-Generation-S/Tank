using TankEngine.DataProvider.Loader;

namespace TankEngine.DataProvider
{
    /// <summary>
    /// Data manger interface
    /// </summary>
    /// <typeparam name="T">The type of data used for this manager</typeparam>
    public interface IDataManager<T> : IDataLoader<T>
    {
        /// <summary>
        /// Clear the data stored in the manager
        /// </summary>
        void Clear();
    }
}
using TankEngine.DataProvider.Saver;

namespace TankEngine.DataProvider
{
    /// <summary>
    /// Saveable Data manager
    /// </summary>
    /// <typeparam name="T">Saveable data manager to use</typeparam>
    public interface ISaveableDataManager<T> : IDataManager<T>, IDataSaver<T>
    {
    }
}
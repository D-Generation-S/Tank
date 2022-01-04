using TankEngine.DataProvider.Saver;

namespace TankEngine.DataProvider
{
    public interface ISaveableDataManager<T> : IDataManager<T>, IDataSaver<T>
    {
    }
}
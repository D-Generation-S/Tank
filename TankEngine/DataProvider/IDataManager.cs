using TankEngine.DataProvider.Loader;

namespace TankEngine.DataProvider
{
    public interface IDataManager<T> : IDataLoader<T>
    {
        void Clear();
    }
}
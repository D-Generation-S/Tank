using System.Threading.Tasks;

namespace Tank.DataManagement
{
    public interface ISaveableDataManager<T> : IDataManager<T>
    {
        bool SaveData(T data, string filename);

        Task<bool> SaveDataAsync(T data, string filename);
    }
}
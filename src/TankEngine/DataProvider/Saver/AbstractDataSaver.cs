using System.Threading.Tasks;

namespace TankEngine.DataProvider.Saver
{
    /// <summary>
    /// Abstract data saver class, use this if you do not want to implement the async method on your own
    /// </summary>
    /// <typeparam name="T">The type of data to load</typeparam>
    public abstract class AbstractDataSaver<T> : IDataSaver<T>
    {
        /// <inheritdoc/>
        public abstract bool SaveData(T dataToSave, string fileName);

        /// <inheritdoc/>
        public async Task<bool> SaveDataAsync(T dataToSave, string filename)
        {
            return await new Task<bool>(() => SaveData(dataToSave, filename));
        }
    }
}

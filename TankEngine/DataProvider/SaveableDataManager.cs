using System.Threading.Tasks;
using Tank.DataManagement.Loader;
using Tank.DataManagement.Saver;

namespace Tank.DataManagement
{
    /// <summary>
    /// Datamanager which can load and save data
    /// </summary>
    /// <typeparam name="T">The type of the data manager</typeparam>
    public class SaveableDataManager<T> : DataManager<T>, ISaveableDataManager<T>
    {
        /// <summary>
        /// The class to use for saving
        /// </summary>
        private readonly IDataSaver<T> dataSaver;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataLoader">The data loader to use</param>
        /// <param name="dataSaver">The data saver to use</param>
        public SaveableDataManager(IDataLoader<T> dataLoader, IDataSaver<T> dataSaver) : base(dataLoader, false)
        {
            this.dataSaver = dataSaver;
        }

        /// <summary>
        /// Save the data to a given file
        /// </summary>
        /// <param name="data">The data to store</param>
        /// <param name="filename">The filename to store the data in</param>
        /// <returns>True if storing was successful</returns>
        public bool SaveData(T data, string filename)
        {
            return dataSaver.SaveData(data, filename);
        }

        /// <inheritdoc/>
        public async Task<bool> SaveDataAsync(T data, string filename)
        {
            return await Task.Run(() => SaveData(data, filename));
        }
    }
}

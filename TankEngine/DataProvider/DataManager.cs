using System.Collections.Generic;
using System.Threading.Tasks;
using TankEngine.DataProvider;
using TankEngine.DataProvider.Loader;
using TankEngine.EntityComponentSystem;

namespace Tank.DataManagement
{
    /// <summary>
    /// The data manager class to get data of spefic type
    /// </summary>
    /// <typeparam name="T">The type of the data manager</typeparam>
    public class DataManager<T> : IClearable, IDataManager<T>
    {
        /// <summary>
        /// The data loader to use
        /// </summary>
        private readonly IDataLoader<T> dataLoader;

        /// <summary>
        /// Should the data be cached
        /// </summary>
        private readonly bool cacheData;

        /// <summary>
        /// The cached data
        /// </summary>
        private readonly Dictionary<string, T> dataCache;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataLoader">The data loader to use</param>
        public DataManager(IDataLoader<T> dataLoader)
            : this(dataLoader, true)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataLoader">The data loader to use</param>
        /// <param name="cacheData">Should the data be cached</param>
        public DataManager(IDataLoader<T> dataLoader, bool cacheData)
        {
            this.dataLoader = dataLoader;
            this.cacheData = cacheData;
            if (cacheData)
            {
                dataCache = new Dictionary<string, T>();
            }
        }

        /// <summary>
        /// Get the data from a speficit file name
        /// </summary>
        /// <param name="fileName">The filename to get the data from</param>
        /// <returns>The requested data</returns>
        public T GetData(string fileName)
        {
            T dataToReturn;
            if (cacheData && dataCache.ContainsKey(fileName))
            {
                dataToReturn = dataCache[fileName];
                return dataToReturn;
            }

            dataToReturn = dataLoader.LoadData(fileName);
            if (cacheData)
            {
                dataCache.Add(fileName, dataToReturn);
            }
            return dataToReturn;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if (cacheData)
            {
                dataCache.Clear();
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetDataAsync(string fileName)
        {
            return await Task.Run(() => GetData(fileName));
        }

        /// <inheritdoc/>
        public C GetData<C>(string fileName, System.Func<T, C> dataConversion)
        {
            return dataConversion(GetData(fileName));
        }

        /// <inheritdoc/>
        public async Task<C> GetDataAsync<C>(string fileName, System.Func<T, C> dataConversion)
        {
            return await Task.Run(async () =>
            {
                T data = await GetDataAsync(fileName);
                return dataConversion(data);
            });
        }
    }
}

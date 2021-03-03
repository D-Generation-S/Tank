using System.Collections.Generic;
using Tank.DataManagement.Loader;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Wrapper;

namespace Tank.DataManagement
{
    /// <summary>
    /// The data manager klass to get data of spefici type
    /// </summary>
    /// <typeparam name="T">The type of the data manager</typeparam>
    class DataManager<T> : IClearable
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
        /// <param name="contentWrapper">The content wrapper to use</param>
        /// <param name="dataLoader">The data loader to use</param>
        public DataManager(ContentWrapper contentWrapper, IDataLoader<T> dataLoader)
            :this(contentWrapper, dataLoader, true)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="contentWrapper">The content wrapper to use</param>
        /// <param name="dataLoader">The data loader to use</param>
        /// <param name="cacheData">Should the data be cached</param>
        public DataManager(ContentWrapper contentWrapper, IDataLoader<T> dataLoader, bool cacheData)
        {
            dataLoader.Init(contentWrapper);
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
    }
}

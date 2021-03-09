using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataManagement.Loader;
using Tank.DataManagement.Saver;
using Tank.Wrapper;

namespace Tank.DataManagement
{
    /// <summary>
    /// Datamanager which can load and save data
    /// </summary>
    /// <typeparam name="T">The type of the data manager</typeparam>
    class SaveableDataManager<T> : DataManager<T>
    {
        /// <summary>
        /// The class to use for saving
        /// </summary>
        private readonly IDataSaver<T> dataSaver;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="contentWrapper">The content wrapper to use</param>
        /// <param name="dataLoader">The data loader to use</param>
        /// <param name="dataSaver">The data saver to use</param>
        public SaveableDataManager(ContentWrapper contentWrapper, IDataLoader<T> dataLoader, IDataSaver<T> dataSaver) : base(contentWrapper, dataLoader, false)
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
    }
}

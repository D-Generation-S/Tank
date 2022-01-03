using System;

namespace TankEngine.DataProvider.Loader
{
    /// <summary>
    /// A simple data loader interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataLoader<T>
    {
        /// <summary>
        /// Load the requested data
        /// </summary>
        /// <param name="fileName">The filename to get the data from</param>
        /// <returns>The requested data</returns>
        T LoadData(string fileName);

        /// <summary>
        /// Load the requested data but convert it to something else
        /// </summary>
        /// <param name="fileName">The filename to get the data from</param>
        /// <param name="dataConversion">The method used to convert the data</param>
        /// <returns>The requested data</returns>
        C LoadData<C>(string fileName, Func<T, C> dataConversion);
    }
}

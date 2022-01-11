using System;
using System.Threading.Tasks;

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
        /// <typeparam name="C">The type to convert the data to</typeparam>
        /// <returns>The requested data</returns>
        C LoadData<C>(string fileName, Func<T, C> dataConversion);

        /// <summary>
        /// Load the requested data async
        /// </summary>
        /// <param name="fileName">The filename to get the data from</param>
        /// <returns>The requested data loading task</returns>
        Task<T> LoadDataAsync(string fileName);

        /// <summary>
        /// Load the requested data ansync and convert it to something else
        /// </summary>
        /// <param name="fileName">The filename to get the data from</param>
        /// <param name="dataConversion">The method used to convert the data</param>
        /// <typeparam name="C">The type to convert the data to</typeparam>
        /// <returns>The requested data</returns>
        Task<C> LoadDataAsync<C>(string fileName, Func<T, C> dataConversion);
    }
}

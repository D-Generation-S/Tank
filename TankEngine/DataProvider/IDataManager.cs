using System;
using System.Threading.Tasks;

namespace TankEngine.DataProvider
{
    public interface IDataManager<T>
    {
        void Clear();

        /// <summary>
        /// Get some data
        /// </summary>
        /// <param name="fileName">The file to get the data from</param>
        /// <returns>The loaded dataset</returns>
        T GetData(string fileName);

        /// <summary>
        /// Get some data async
        /// </summary>
        /// <param name="fileName">The file to get the data from</param>
        /// <returns>The loaded dataset</returns>
        Task<T> GetDataAsync(string fileName);

        /// <summary>
        /// Get some converted data
        /// </summary>
        /// <param name="fileName">The filename to get the data from</param>
        /// <param name="dataConversion">The method to use for the data conversion</param>
        /// <typeparam name="C">The type to convert the data to</typeparam>
        /// <returns>The converted data</returns>
        C GetData<C>(string fileName, Func<T, C> dataConversion);

        /// <summary>
        /// Get some converted data async
        /// </summary>
        /// <param name="fileName">The filename to get the data from</param>
        /// <param name="dataConversion">The method to use for the data conversion</param>
        /// <typeparam name="C">The type to convert the data to</typeparam>
        /// <returns>The converted data</returns>
        Task<C> GetDataAsync<C>(string fileName, Func<T, C> dataConversion);
    }
}
using System.Threading.Tasks;

namespace TankEngine.DataProvider.Converters
{
    /// <summary>
    /// Interface to convert data 
    /// </summary>
    /// <typeparam name="D">The data which should be converted</typeparam>
    /// <typeparam name="T">The object the data should be converted to</typeparam>
    public interface IDataConverter<in D, T>
    {
        /// <summary>
        /// Convert the data
        /// </summary>
        /// <param name="data">The data to convert</param>
        /// <returns>The converted data</returns>
        T Convert(D data);

        /// <summary>
        /// Async method to convert the data
        /// </summary>
        /// <param name="data">he data to convert</param>
        /// <returns>A awaitable task with the converted data</returns>
        Task<T> ConvertAsync(D data);
    }
}

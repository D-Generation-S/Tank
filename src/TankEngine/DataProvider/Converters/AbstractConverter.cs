using System.Threading.Tasks;

namespace TankEngine.DataProvider.Converters
{
    /// <summary>
    /// Abstract data conversion which implements the async methods
    /// </summary>
    /// <typeparam name="D">The type of data which should be converted</typeparam>
    /// <typeparam name="T">The type of data which should be converted to</typeparam>
    public abstract class AbstractConverter<D, T> : IDataConverter<D, T>
    {
        /// <inheritdoc/>
        public abstract T Convert(D value);

        /// <inheritdoc/>
        public virtual async Task<T> ConvertAsync(D value)
        {
            return await Task.Run(() => Convert(value));
        }
    }
}

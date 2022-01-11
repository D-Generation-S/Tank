using System;
using System.Threading.Tasks;

namespace TankEngine.DataProvider.Converters
{
    /// <summary>
    /// Class to allow conversion from a dynamic function
    /// </summary>
    /// <typeparam name="D">The input data to convert</typeparam>
    /// <typeparam name="T">The datatype to output</typeparam>
    internal class DynamicActionConverter<D, T> : IDataConverter<D, T>
    {
        /// <summary>
        /// Function used to convert the data
        /// </summary>
        private readonly Func<D, T> conversionFunction;

        /// <inheritdoc/>
        public DynamicActionConverter(Func<D, T> conversionFunction)
        {
            this.conversionFunction = conversionFunction;
        }

        /// <inheritdoc/>
        public T Convert(D dataIn)
        {
            return conversionFunction == null ? default(T) : conversionFunction(dataIn);
        }

        /// <inheritdoc/>
        public async Task<T> ConvertAsync(D dataIn)
        {
            return await Task.Run(() => conversionFunction == null ? default(T) : conversionFunction(dataIn));
        }
    }
}

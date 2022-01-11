using System;
using System.Threading.Tasks;
using TankEngine.DataProvider.Converters;

namespace TankEngine.DataProvider.Loader
{
    /// <summary>
    /// Dynamic data loader to mix and match loader and conversion
    /// </summary>
    /// <typeparam name="T">The data type which should be the result after loading</typeparam>
    /// <typeparam name="L">The type of data which is resulting from loading the data</typeparam>
    public class DynamicDataLoader<T, L> : AbstractDataLoader<T>
    {
        /// <summary>
        /// The data loader to use
        /// </summary>
        private readonly IDataLoader<L> dataLoader;

        /// <summary>
        /// The data converter to use
        /// </summary>
        private readonly IDataConverter<L, T> dataConverter;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataLoader">The data loader to use</param>
        /// <param name="dataConverter">The data converter to use</param>
        public DynamicDataLoader(IDataLoader<L> dataLoader, IDataConverter<L, T> dataConverter)
        {
            this.dataLoader = dataLoader;
            this.dataConverter = dataConverter;
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataLoader">The data loader to use</param>
        /// <param name="conversionMethod">The conversion method to use</param>
        public DynamicDataLoader(IDataLoader<L> dataLoader, Func<L, T> conversionMethod)
            : this(dataLoader, new DynamicActionConverter<L, T>(conversionMethod)) { }

        /// <inheritdoc/>
        public override T LoadData(string fileName)
        {
            if (dataLoader == null || dataConverter == null)
            {
                return default(T);
            }
            L loadedData = dataLoader.LoadData(fileName);
            return dataConverter.Convert(loadedData);
        }

        /// <inheritdoc/>
        public override async Task<T> LoadDataAsync(string fileName)
        {
            if (dataLoader == null || dataConverter == null)
            {
                return default(T);
            }
            L loadedData = await dataLoader.LoadDataAsync(fileName);
            return await dataConverter.ConvertAsync(loadedData);
        }
    }
}

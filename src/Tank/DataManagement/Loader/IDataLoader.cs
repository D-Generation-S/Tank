using Tank.Wrapper;

namespace Tank.DataManagement.Loader
{
    /// <summary>
    /// A simple data loader interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IDataLoader<T>
    {
        /// <summary>
        /// Initzialize the data loader with the content wrapper
        /// </summary>
        /// <param name="contentWrapper">The content wrapper to use</param>
        void Init(ContentWrapper contentWrapper);

        /// <summary>
        /// Load the requested data
        /// </summary>
        /// <param name="fileName">The filename to get the data from</param>
        /// <returns>The requested data</returns>
        T LoadData(string fileName);
    }
}

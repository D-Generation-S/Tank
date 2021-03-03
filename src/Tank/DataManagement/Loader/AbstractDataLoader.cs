using System.IO;
using System.Reflection;
using Tank.Wrapper;

namespace Tank.DataManagement.Loader
{
    /// <summary>
    /// The abstract data loader class
    /// </summary>
    /// <typeparam name="T">The type of the data loader</typeparam>
    abstract class AbstractDataLoader<T> : IDataLoader<T>
    {
        /// <summary>
        /// The content wrapper to use for loading
        /// </summary>
        protected ContentWrapper contentWrapper;

        /// <inheritdoc/>
        public virtual void Init(ContentWrapper contentWrapper)
        {
            this.contentWrapper = contentWrapper;
        }

        /// <summary>
        /// Get the path to the game data root folder
        /// </summary>
        /// <returns>The game data root folder</returns>
        protected virtual string GetGameDataFolder()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            FileInfo fileInfo = new FileInfo(assemblyLocation);
            return Path.Combine(fileInfo.DirectoryName, "GameData");
        }

        /// <inheritdoc/>
        public abstract T LoadData(string fileName);
    }
}

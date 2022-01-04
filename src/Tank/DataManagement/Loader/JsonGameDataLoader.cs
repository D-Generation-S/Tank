using System.IO;
using System.Reflection;
using TankEngine.DataProvider.Loader;

namespace Tank.DataManagement.Loader
{
    /// <summary>
    /// Json data loader using the "game data" folder as a root directory
    /// </summary>
    /// <typeparam name="T">The type of data to load</typeparam>
    internal class JsonGameDataLoader<T> : JsonDataLoader<T>
    {
        /// <summary>
        /// The sub folder to use on the game data folder
        /// </summary>
        public string SubFolder { get; }

        /// <inheritdoc/>
        public JsonGameDataLoader(string subFolder)
        {
            SubFolder = subFolder;
        }

        /// <inheritdoc/>
        public override T LoadData(string fileName)
        {
            fileName = Path.Combine(GetGameDataFolder(), fileName);
            return base.LoadData(fileName);
        }

        /// <summary>
        /// Get the game data folder
        /// </summary>
        /// <returns>The game data folder</returns>
        public string GetGameDataFolder()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            FileInfo fileInfo = new FileInfo(assemblyLocation);
            string folder = Path.Combine(fileInfo.DirectoryName, "GameData");
            if (SubFolder == string.Empty)
            {
                return folder;
            }
            return Path.Combine(folder, SubFolder);
        }
    }
}

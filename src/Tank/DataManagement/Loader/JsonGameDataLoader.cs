using System.IO;
using System.Reflection;
using TankEngine.DataProvider.Loader;

namespace Tank.DataManagement.Loader
{
    internal class JsonGameDataLoader<T> : JsonDataLoader<T>
    {
        public string SubFolder { get; }

        public JsonGameDataLoader(string subFolder)
        {
            SubFolder = subFolder;
        }

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

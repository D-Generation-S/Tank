using System;
using System.IO;
using System.Reflection;

namespace Tank.Utils
{
    /// <summary>
    /// Utilities to get different folder paths
    /// </summary>
    class DefaultFolderUtils
    {
        /// <summary>
        /// Path to the settings folder
        /// </summary>
        private string settingFolder;

        public DefaultFolderUtils()
        {
            settingFolder = string.Empty;
        }

        /// <summary>
        /// Get the settings base folder
        /// </summary>
        /// <returns>Get the application folder</returns>
        public string GetApplicationFolder()
        {
            if (settingFolder == string.Empty)
            {
                settingFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }

            return settingFolder;
        }

        /// <summary>
        /// Get the path to the game folder
        /// </summary>
        /// <returns>The path to the game folder</returns>
        public string GetGameFolder()
        {
            return Path.Combine(GetApplicationFolder(), "TankGame");
        }

        /// <summary>
        /// Get the game data folder
        /// </summary>
        /// <returns></returns>
        public string GetGameDataFolder()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            FileInfo fileInfo = new FileInfo(assemblyLocation);
            return Path.Combine(fileInfo.DirectoryName, "GameData");
        }

        /// <summary>
        /// Get the game data folder with a given subfolder
        /// </summary>
        /// <param name="subFolder">The subfolder to use</param>
        /// <returns>The path with the given subfolder</returns>
        public string GetGameDataFolder(string subFolder)
        {
            return Path.Combine(GetGameDataFolder(), subFolder);
        }


        /// <summary>
        /// Get the game data folder with a given subfolder and a specific file
        /// </summary>
        /// <param name="subFolder">The subfolder to use</param>
        /// <param name="fileName">The filename to use</param>
        /// <returns>The path with the given subfolder and file</returns>
        public string GetGameDataFolder(string subFolder, string fileName)
        {
            return Path.Combine(GetGameDataFolder(subFolder), fileName);
        }

    }
}

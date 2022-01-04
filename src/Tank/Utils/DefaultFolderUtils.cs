using System;
using System.IO;

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
    }
}

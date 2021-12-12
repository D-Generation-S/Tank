using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tank.Utils
{
    class DefaultFolderUtils
    {
        private string settingFolder;

        public DefaultFolderUtils()
        {
            settingFolder = string.Empty;
        }

        /// <summary>
        /// Get the settings base folder
        /// </summary>
        /// <returns></returns>
        public string GetApplicationFolder()
        {
            if (settingFolder == string.Empty)
            {
                settingFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }

            return settingFolder;
        }

        public string GetGameFolder()
        {
            return Path.Combine(GetApplicationFolder(), "TankGame");
        }
    }
}

using Newtonsoft.Json;
using System;
using System.IO;
using Tank.DataStructure.Settings;
using Tank.Utils;

namespace Tank.DataManagement.Saver
{
    /// <summary>
    /// Save the application settings as json
    /// </summary>
    class JsonSettingSaver : IDataSaver<ApplicationSettings>
    {
        /// <summary>
        /// Utility to get default folders
        /// </summary>
        private DefaultFolderUtils folderUtils;

        public JsonSettingSaver()
        {
            folderUtils = new DefaultFolderUtils();
        }

        /// <inheritdoc/>
        public bool SaveData(ApplicationSettings dataToSave, string fileName)
        {
            string directoryName = GetSettingFolder();
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            string realFileName = Path.Combine(directoryName, fileName + ".json");
            
            using (StreamWriter writer = new StreamWriter(realFileName))
            {
                try
                {
                    string dataToWrite = JsonConvert.SerializeObject(dataToSave);
                    writer.Write(dataToWrite);
                }
                catch (Exception)
                {
                    return false;
                    ///Loading did fail returning null
                }
            }

            return true;
        }

        /// <summary>
        /// Get the settings folder path
        /// </summary>
        /// <returns>The path to the setting folder</returns>
        public string GetSettingFolder()
        {
            return Path.Combine(folderUtils.GetApplicationFolder(), TankGame.GameName);

        }
    }
}

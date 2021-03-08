using Newtonsoft.Json;
using System;
using System.IO;
using Tank.DataStructure.Settings;

namespace Tank.DataManagement.Loader
{
    /// <summary>
    /// Class to load the application setting container
    /// </summary>
    class JsonSettingLoader : AbstractDataLoader<ApplicationSettings>
    {
        /// <inheritdoc/>
        public override ApplicationSettings LoadData(string fileName)
        {
            ApplicationSettings returnSettings = new ApplicationSettings();

            string realFileName = Path.Combine(GetSettingFolder(), fileName + ".json");
            if (!File.Exists(realFileName))
            {
                return returnSettings;
            }

            using (StreamReader reader = new StreamReader(realFileName))
            {
                string readData = reader.ReadToEnd();
                try
                {
                    ApplicationSettings data = JsonConvert.DeserializeObject<ApplicationSettings>(readData);
                    if (data != null)
                    {
                        returnSettings = data;
                    }
                    
                }
                catch (Exception)
                {
                    ///Loading did fail returning null
                }

            }
            return returnSettings;
        }

        /// <summary>
        /// Get the settings folder path
        /// </summary>
        /// <returns>The path to the setting folder</returns>
        public string GetSettingFolder()
        {
            string returnFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(returnFolder, TankGame.GameName);

        }
    }
}

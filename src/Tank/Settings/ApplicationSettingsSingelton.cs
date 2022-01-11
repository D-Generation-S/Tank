using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using Tank.Settings;
using Tank.Utils;
using TankEngine.DataProvider.Loader;
using TankEngine.DataProvider.Saver;
using TankEngine.DataStructures.Serializeable;

namespace Tank.DataStructure.Settings
{
    /// <summary>
    /// The settings for the whole application
    /// This is a singelton class
    /// </summary>
    public sealed class ApplicationSettingsSingelton
    {
        /// <summary>
        /// The name of the settings file
        /// </summary>
        private const string SETTING_FILE_NAME = "settings.json";

        /// <summary>
        /// Is the application in fullscreen
        /// </summary>
        public bool FullScreen;

        /// <summary>
        /// The resolution of the application
        /// </summary>
        public Point Resolution;

        /// <summary>
        /// The percentage of the master volume
        /// </summary>
        public int MasterVolumePercent;

        /// <summary>
        /// The usable master volume
        /// </summary>
        public float MasterVolume => (float)MasterVolumePercent / 100f;

        /// <summary>
        /// The percentage of the music volume
        /// </summary>
        public int MusicVolumePercent;

        /// <summary>
        /// The useable music volume
        /// </summary>
        public float MusicVolume => ((float)MusicVolumePercent / 100f) * MasterVolume;

        /// <summary>
        /// The percentage of the effect volume
        /// </summary>
        public int EffectVolumePercent;

        /// <summary>
        /// The effect volume
        /// </summary>
        public float EffectVolume => ((float)EffectVolumePercent / 100f) * MasterVolume;

        /// <summary>
        /// The last played song
        /// </summary>
        public Song LastPlayedSong;

        /// <summary>
        /// The timestamp of the last played song
        /// </summary>
        public TimeSpan LastTimeSpan;

        /// <summary>
        /// Utility to get default folders
        /// </summary>
        private readonly DefaultFolderUtils folderUtils;

        /// <summary>
        /// Settings private singelton instance
        /// </summary>
        private static ApplicationSettingsSingelton instance;

        /// <summary>
        /// Data loader to use for the settings
        /// </summary>
        private IDataLoader<SerializeableSettings> dataLoader;

        /// <summary>
        /// Data saver used for the settings
        /// </summary>
        private IDataSaver<SerializeableSettings> dataSaver;



        /// <summary>
        /// Settings public instance accessor
        /// </summary>
        public static ApplicationSettingsSingelton Instance
        {
            get { return instance ??= new ApplicationSettingsSingelton(); }
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        private ApplicationSettingsSingelton()
        {
            MasterVolumePercent = 100;
            MusicVolumePercent = 100;
            EffectVolumePercent = 100;
            Resolution = new Point(1280, 720);
            FullScreen = false;
            instance = null;
            folderUtils = new DefaultFolderUtils();

            dataLoader = new JsonDataLoader<SerializeableSettings>();
            dataSaver = new JsonDataSaver<SerializeableSettings>();

        }

        /// <summary>
        /// Load the game settings
        /// </summary>
        /// <returns>True if loading was successful</returns>
        public bool Load()
        {
            if (!File.Exists(GetSettingFileName()))
            {
                return false;
            }
            SerializeableSettings settings = dataLoader.LoadData(GetSettingFileName());
            if (settings == null)
            {
                return false;
            }
            FullScreen = settings.FullScreen;
            Resolution = settings.Resolution.GetPoint();
            MasterVolumePercent = settings.MasterVolumePercent;
            EffectVolumePercent = settings.EffectVolumePercent;
            MusicVolumePercent = settings.MusicVolumePercent;

            return true;
        }

        /// <summary>
        /// Save the application settings
        /// </summary>
        /// <returns>True if saving was successful</returns>
        public bool Save()
        {
            if (!Directory.Exists(GetSettingFolder()))
            {
                Directory.CreateDirectory(GetSettingFolder());
            }
            SerializeableSettings settings = new SerializeableSettings()
            {
                FullScreen = FullScreen,
                Resolution = new SPoint(Resolution),
                MasterVolumePercent = MasterVolumePercent,
                EffectVolumePercent = EffectVolumePercent,
                MusicVolumePercent = MusicVolumePercent
            };
            return dataSaver.SaveData(settings, GetSettingFileName());
        }

        /// <summary>
        /// Get the path to the settings file
        /// </summary>
        /// <returns>The full path of the settings file</returns>
        public string GetSettingFileName()
        {
            return Path.Combine(GetSettingFolder(), SETTING_FILE_NAME);
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

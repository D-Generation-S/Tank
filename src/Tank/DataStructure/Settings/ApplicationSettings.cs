using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System;

namespace Tank.DataStructure.Settings
{
    /// <summary>
    /// The settings for the whole application
    /// </summary>
    class ApplicationSettings
    {
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
        [JsonIgnore]
        public float MasterVolume => (float)MasterVolumePercent / 100f;

        /// <summary>
        /// The percentage of the music volume
        /// </summary>
        public int MusicVolumePercent;

        /// <summary>
        /// The useable music volume
        /// </summary>
        [JsonIgnore]
        public float MusicVolume => ((float)MusicVolumePercent / 100f) * MasterVolume;

        /// <summary>
        /// The percentage of the effect volume
        /// </summary>
        public int EffectVolumePercent;

        /// <summary>
        /// The effect volume
        /// </summary>
        [JsonIgnore]
        public float EffectVolume => ((float)EffectVolumePercent / 100f) * MasterVolume;

        /// <summary>
        /// The last played song
        /// </summary>
        [JsonIgnore]
        public Song LastPlayedSong;

        /// <summary>
        /// The timestamp of the last played song
        /// </summary>
        [JsonIgnore]
        public TimeSpan LastTimeSpan;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public ApplicationSettings()
        {
            MasterVolumePercent = 100;
            MusicVolumePercent = 100;
            EffectVolumePercent = 100;
            Resolution = new Point(1280, 720);
            FullScreen = false;
        }
    }
}

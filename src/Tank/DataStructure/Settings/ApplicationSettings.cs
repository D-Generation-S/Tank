using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Tank.DataStructure.Settings
{
    class ApplicationSettings
    {
        public bool FullScreen;

        public Point Resolution;

        public int MasterVolumePercent;

        [JsonIgnore]
        public float MasterVolume => (float)MasterVolumePercent / 100f;

        public int MusicVolumePercent;

        [JsonIgnore]
        public float MusicVolume => ((float)MusicVolumePercent / 100f) * MasterVolume;

        public int EffectVolumePercent;

        [JsonIgnore]
        public float EffectVolume => ((float)EffectVolumePercent / 100f) * MasterVolume;

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

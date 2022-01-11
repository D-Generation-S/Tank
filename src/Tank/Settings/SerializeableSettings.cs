using TankEngine.DataStructures.Serializeable;

namespace Tank.Settings
{
    /// <summary>
    /// Class used for settings serialization
    /// </summary>
    public class SerializeableSettings
    {
        /// <summary>
        /// Is application fullscreen
        /// </summary>
        public bool FullScreen { get; set; }

        /// <summary>
        /// Resolution for the game
        /// </summary>
        public SPoint Resolution { get; set; }

        /// <summary>
        /// Percentage of the master volume
        /// </summary>
        public int MasterVolumePercent { get; set; }

        /// <summary>
        /// Percentage of the music volume
        /// </summary>
        public int MusicVolumePercent { get; set; }

        /// <summary>
        /// Percentage of the effect volume
        /// </summary>
        public int EffectVolumePercent { get; set; }
    }
}

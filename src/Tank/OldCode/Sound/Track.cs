using Microsoft.Xna.Framework.Media;
using Tank.Enums;

namespace Tank.Code.Sound
{
    public class Track
    {
        public Song Audio
        {
            get; set;
        }

        public TrackType Category
        {
            get; set;
        }

        public Track(Song audio, TrackType type = TrackType.Mute)
        {
            Audio = audio;
            Category = type;
        }
    }
}

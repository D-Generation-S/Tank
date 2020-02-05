using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

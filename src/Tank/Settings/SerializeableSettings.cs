using Microsoft.Xna.Framework;

namespace Tank.Settings
{
    public class SerializeableSettings
    {
        public bool FullScreen { get; set; }
        public Resolution Resolution { get; set; }
        public int MasterVolumePercent { get; set; }
        public int MusicVolumePercent { get; set; }
        public int EffectVolumePercent { get; set; }
    }

    public struct Resolution
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point GetResolution()
        {
            return new Point(X, Y);
        }
    }
}

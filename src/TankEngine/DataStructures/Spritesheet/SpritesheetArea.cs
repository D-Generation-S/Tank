using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TankEngine.DataStructures.Spritesheet
{
    public class SpritesheetArea
    {
        public string Name { get; }

        public List<SpritesheetProperty> Properties { get; }

        public int FrameNumber { get; }

        public Rectangle Area { get; }

        public SpritesheetArea(string name, List<SpritesheetProperty> properties, int frameNumber, Rectangle area)
        {
            Name = name;
            Properties = properties;
            FrameNumber = frameNumber;
            Area = area;
        }
    }
}

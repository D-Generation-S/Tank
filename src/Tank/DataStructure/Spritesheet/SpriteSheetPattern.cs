using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.DataStructure.Spritesheet
{
    class SpriteSheetPattern
    {
        public string Name { get; }
        public Position position { get; }

        public SpriteSheetPattern(string name, Position position)
        {
            Name = name;
            this.position = position;
        }
    }
}

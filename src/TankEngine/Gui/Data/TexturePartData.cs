using System;
using System.Collections.Generic;
using System.Linq;
using TankEngine.DataStructures.Spritesheet;

namespace TankEngine.Gui.Data
{
    public class TexturePartData
    {
        public List<SpritesheetArea> Areas;

        public TexturePartData(ISpritesheetData spritesheetData, Func<List<SpritesheetArea>, IEnumerable<SpritesheetArea>> getValidAreas)
        {
            Areas = getValidAreas(spritesheetData.Areas).ToList();
        }
    }
}

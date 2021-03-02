using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataStructure;
using Tank.DataStructure.Spritesheet;
using Tank.Wrapper;

namespace Tank.DataManagement.Loader
{
    class MapTextureMockLoader : AbstractDataLoader<SpriteSheet>
    {
        public override SpriteSheet LoadData(string fileName)
        {
            SpriteSheet sheet = new SpriteSheet(contentWrapper.Content.Load<Texture2D>("Images/Textures/MoistContinentalSpritesheet"), new Position(32, 32), 0);
            sheet.SetSpriteSheetPattern(new List<SpriteSheetPattern>()
            {
                new SpriteSheetPattern("dirt", new Position(1,1)),
                new SpriteSheetPattern("stone", new Position(0,1))
            });

            return sheet;
        }
    }
}

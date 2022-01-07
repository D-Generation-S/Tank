using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Tank.Components;
using Tank.Interfaces.MapGenerators;
using TankEngine.DataStructures;
using TankEngine.DataStructures.Spritesheet;
using TankEngine.Randomizer;

namespace Tank.Map.Textureizer
{
    /// <summary>
    /// This class will texturize the map with one texture only
    /// </summary>
    class SimpleTexturizer : IMapTexturizer
    {
        /// <summary>
        /// The spritesheet to use
        /// </summary>
        private readonly SpritesheetTexture terrainSpritesheet;

        /// <summary>
        /// The randomizer to use
        /// </summary>
        private IRandomizer randomizer;

        /// <summary>
        /// The y position of the sprite to use
        /// </summary>
        private int spriteYPosition;

        /// <summary>
        /// The x position of the sprite to use
        /// </summary>
        private int spriteXPosition;

        /// <summary>
        /// Create a new instance of the class
        /// </summary>
        /// <param name="backgroundTexture">The spritesheet to use</param>
        public SimpleTexturizer(SpritesheetTexture backgroundTexture)
        {
            terrainSpritesheet = backgroundTexture;
        }

        /// <summary>
        /// Texturize the map
        /// </summary>
        /// <param name="map">The map to texturize</param>
        /// <param name="generatorFillColor">The color the generator did use for drawing the map template</param>
        public void TexturizeMap(MapComponent map, Color generatorFillColor)
        {
            TexturizeMap(map, generatorFillColor, null);
        }

        /// <summary>
        /// Texturize the map
        /// </summary>
        /// <param name="map">The map to texturize</param>
        /// <param name="generatorFillColor">The color the generator did use for drawing the map template</param>
        /// <param name="randomizer">The randomizer to use</param>
        public void TexturizeMap(MapComponent map, Color generatorFillColor, IRandomizer randomizer)
        {
            spriteYPosition = 0;
            spriteXPosition = 0;

            this.randomizer = randomizer ?? new SystemRandomizer();
            IEnumerable<SpritesheetArea> backgroundAreas = terrainSpritesheet.Areas.Where(area => area.ContainsProperty("type", "background", false));
            SpritesheetArea areaToUse = backgroundAreas.ElementAt(this.randomizer.GetNewIntNumber(0, backgroundAreas.Count()));
            FlattenArray<Color> colors = terrainSpritesheet.GetColorFromArea(areaToUse);

            for (int x = 0; x < map.Width; x++)
            {
                if (spriteXPosition > areaToUse.Area.Width - 1)
                {
                    spriteXPosition = 0;
                }
                for (int y = 0; y < map.Height; y++)
                {
                    if (spriteYPosition > areaToUse.Area.Height - 1)
                    {
                        spriteYPosition = 0;
                    }

                    if (map.ImageData.GetValue(x, y) == generatorFillColor)
                    {
                        map.ImageData.SetValue(x, y, colors.GetValue(spriteXPosition, spriteYPosition));
                        spriteYPosition++;
                    }
                }
                spriteXPosition++;
                spriteYPosition = 0;
            }
        }
    }
}

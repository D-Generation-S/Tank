using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Tank.Components;
using Tank.Interfaces.MapGenerators;
using TankEngine.DataStructures;
using TankEngine.DataStructures.Grid;
using TankEngine.DataStructures.Spritesheet;
using TankEngine.Loottable;
using TankEngine.Randomizer;

namespace Tank.Map.Textureizer
{
    /// <summary>
    /// This class will texturize the map with one texture only
    /// </summary>
    class SimpleTexturizer : IMapTexturizer
    {
        private const int MIN_REQUIRED_ENTITY_PLACE = 32;

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
            int validPixels = 0;
            int gridWidth = (int)Math.Ceiling((double)map.Width / areaToUse.Area.Width);
            int gridHeight = (int)Math.Ceiling((double)map.Height / areaToUse.Area.Height);
            Grid<int> imageGrid = new Grid<int>(gridWidth, gridHeight, areaToUse.Area.Width, Vector2.Zero, (x, y) => y * gridWidth + x);
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map.ImageData.GetValue(x, y) != generatorFillColor)
                    {
                        continue;
                    }
                    validPixels++;
                    Point grid = imageGrid.GetPositionInGrid(new Vector2(x, y));
                    int xStart = x - (grid.X * areaToUse.Area.Width);
                    int yStart = y - (grid.Y * areaToUse.Area.Height);
                    Color colorToPlace = colors.GetValue(xStart, yStart);
                    map.ImageData.SetValue(x, y, colorToPlace);
                }
            }
            PlaceEntitesUnderGround(map, validPixels);
        }

        /// <summary>
        /// Place some entites on the texturized ground map
        /// </summary>
        /// <param name="map">The current map to work on</param>
        /// <param name="validPixels">The number of valid pixels for the map</param>
        private void PlaceEntitesUnderGround(MapComponent map, int validPixels)
        {
            List<SpritesheetArea> foregroundAreas = terrainSpritesheet.Areas.Where(area => area.ContainsProperty("type", "entity", false) && area.ContainsPropertyName("rarity", false)).ToList();
            int maxEntites = MathHelper.Min(validPixels / (64 * 64), 25);
            int numberOfEntitesToPlace = this.randomizer.GetNewIntNumber(0, maxEntites);
            List<EntityArea> regions = new List<EntityArea>();
            if (numberOfEntitesToPlace == 0 || map.Height - map.LowestPoint < MIN_REQUIRED_ENTITY_PLACE)
            {
                return;
            }
            ILoottable<SpritesheetArea> loottable = CreateLootTable(foregroundAreas);
            while (regions.Count < numberOfEntitesToPlace)
            {
                int xPos = this.randomizer.GetNewIntNumber(0, map.Width);
                int yPos = this.randomizer.GetNewIntNumber((int)map.LowestPoint, map.Height);
                SpritesheetArea areaToPlace = loottable.GetItem();
                if (areaToPlace == null)
                {
                    continue;
                }
                Rectangle area = new Rectangle(xPos, yPos, areaToPlace.Area.Width, areaToPlace.Area.Height);
                if (regions.Any(currentArea => currentArea.targetPosition.Intersects(area)))
                {
                    continue;
                }
                regions.Add(new EntityArea(area, areaToPlace));
            }

            foreach (EntityArea area in regions)
            {
                FlattenArray<Color> partData = terrainSpritesheet.GetColorFromArea(area.sourceTexture);
                for (int x = 0; x < area.sourceTexture.Area.Width; x++)
                {
                    for (int y = 0; y < area.sourceTexture.Area.Height; y++)
                    {
                        int xWorldPos = area.targetPosition.X + x;
                        int yWorldPos = area.targetPosition.Y + y;
                        Color replaceColor = partData.GetValue(x, y);
                        if (replaceColor == Color.Transparent)
                        {
                            continue;
                        }
                        map.ImageData.SetValue(
                            xWorldPos,
                            yWorldPos,
                            partData.GetValue(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Create a loottable for all the entites
        /// </summary>
        /// <param name="foregroundAreas">All the entites to place in foreground</param>
        /// <returns>A ready to use loottable</returns>
        private ILoottable<SpritesheetArea> CreateLootTable(List<SpritesheetArea> foregroundAreas)
        {
            ILoottable<SpritesheetArea> loottable = new SimpleLoottable<SpritesheetArea>(randomizer.GetNewIntNumber(5, 50), randomizer);
            foreach (SpritesheetArea area in foregroundAreas)
            {
                SpritesheetProperty rarity = area.Properties.FirstOrDefault(p => p.Name.ToLower() == "rarity");
                int rarityValue = 0;
                if (!int.TryParse(rarity.Value, out rarityValue))
                {
                    continue;
                }
                loottable.AddItem(area, rarityValue);
            }
            return loottable;
        }

        /// <summary>
        /// Internal structure to conbine world position with spritesheet area
        /// </summary>
        internal struct EntityArea
        {
            /// <summary>
            /// The target position in the world
            /// </summary>
            public Rectangle targetPosition { get; }

            /// <summary>
            /// The position on the texture
            /// </summary>
            public SpritesheetArea sourceTexture { get; }

            /// <summary>
            /// Create a new instance of this class
            /// </summary>
            /// <param name="targetPosition">The target position in the world</param>
            /// <param name="sourceTexture">The position on the texture</param>
            public EntityArea(Rectangle targetPosition, SpritesheetArea sourceTexture)
            {
                this.targetPosition = targetPosition;
                this.sourceTexture = sourceTexture;
            }
        }
    }
}

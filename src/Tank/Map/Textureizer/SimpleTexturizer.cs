using Microsoft.Xna.Framework;
using System;
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
        private readonly SpriteSheet spriteSheet;

        /// <summary>
        /// The foreground items to use
        /// </summary>
        private readonly SpriteSheet foregroundItems;

        /// <summary>
        /// The internal randomizer if nothing was set
        /// </summary>
        private Random internalRandomizer;

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
        public SimpleTexturizer(SpriteSheet backgroundTexture) : this(backgroundTexture, null)
        {
        }

        public SimpleTexturizer(SpriteSheet backgroundTexture, SpriteSheet foregroundItems)
        {
            spriteSheet = backgroundTexture;
            this.foregroundItems = foregroundItems;
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

            this.randomizer = randomizer;
            internalRandomizer = randomizer == null ? new Random(map.Seed) : null;

            FlattenArray<Color> colors = spriteSheet.GetColorByName("stone");

            for (int x = 0; x < map.Width; x++)
            {
                if (spriteXPosition > spriteSheet.SingleImageSize.X - 1)
                {
                    spriteXPosition = 0;
                }
                for (int y = 0; y < map.Height; y++)
                {
                    if (spriteYPosition > spriteSheet.SingleImageSize.Y - 1)
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

        /// <summary>
        /// Get a random number
        /// </summary>
        /// <param name="minimum">The minimum number to generate</param>
        /// <param name="maximum">The maximum number to generate</param>
        /// <returns>A random float number</returns>
        private float GetRandomNumber(float minimum, float maximum)
        {
            if (randomizer != null)
            {
                return randomizer.GetNewNumber(minimum, maximum);
            }

            return (float)internalRandomizer.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}

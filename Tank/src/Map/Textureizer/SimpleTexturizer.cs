using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.DataStructure;
using Tank.src.Interfaces.MapGenerators;
using Tank.src.Interfaces.Randomizer;

namespace Tank.src.Code.Textureizer
{
    class DefaultTextureizer : IMapTexturizer
    {
        private readonly SpriteSheet spriteSheet;
        private Random internalRandomizer;

        private IRandomizer randomizer;

        private int spriteYPosition;
        private int spriteXPosition;

        public DefaultTextureizer(SpriteSheet spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }

        public void TexturizeMap(IMap map, Color generatorFillColor)
        {
            TexturizeMap(map, generatorFillColor, null);
        }

        public void TexturizeMap(IMap map, Color generatorFillColor, IRandomizer randomizer)
        {
            spriteYPosition = 0;
            spriteXPosition = 0;

            this.randomizer = randomizer;
            internalRandomizer = randomizer == null ? new Random(map.Seed) : null;

            Color[] colorsToUse = spriteSheet.GetColorFromSprite(0, 0);
            FlattenArray<Color> colors = new FlattenArray<Color>(colorsToUse, spriteSheet.SingleImageSize.X);

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

                    if (map.GetPixel(x, y) == generatorFillColor)
                    {
                        map.ChangePixel(x, y, colors.GetValue(spriteXPosition, spriteYPosition));
                        spriteYPosition++;
                    }
                }
                spriteXPosition++;
                spriteYPosition = 0;
            }

            map.ApplyChanges();

            return;
        }

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

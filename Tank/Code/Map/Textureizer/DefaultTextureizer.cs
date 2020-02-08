using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.MapGenerators;
using Tank.Interfaces.Random;

namespace Tank.Code.Textureizer
{
    class DefaultTextureizer : IMapTexturizer
    {
        private Random internalRandomizer;

        private IRandom randomizer;

        public DefaultTextureizer(Texture2D spriteSheet)
        {

        }

        public void TexturizeMap(IMap map)
        {
            TexturizeMap(map, null);
        }

        public void TexturizeMap(IMap map, IRandom randomizer)
        {
            this.randomizer = randomizer;
            internalRandomizer = randomizer == null ? new Random(map.Seed) : null;

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

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.Random;

namespace Tank.Interfaces.MapGenerators
{
    interface IMapTexturizer
    {
        /// <summary>
        /// This will texturize a given map
        /// </summary>
        /// <param name="map">The map to texturize</param>
        /// <param name="generatorFillColor">The color the generator did use for creating the map</param>
        void TexturizeMap(IMap map, Color generatorFillColor);

        /// <summary>
        /// This will texturize a given map
        /// </summary>
        /// <param name="map">The map to texturize</param>
        /// <param name="generatorFillColor">The color the generator did use for creating the map</param>
        /// <param name="randomizer">The randomizer to use</param>
        void TexturizeMap(IMap map, Color generatorFillColor, IRandom randomizer);
    }
}

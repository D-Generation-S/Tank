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
        void TexturizeMap(IMap map);

        /// <summary>
        /// This will texturize a given map
        /// </summary>
        /// <param name="map">The map to texturize</param>
        /// <param name="randomizer">The randomizer to use</param>
        void TexturizeMap(IMap map, IRandom randomizer);
    }
}

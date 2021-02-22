using Microsoft.Xna.Framework;
using Tank.Interfaces.Randomizer;

namespace Tank.Interfaces.MapGenerators
{
    /// <summary>
    /// This interface represents a map texturizer used to texturize a map
    /// </summary>
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
        void TexturizeMap(IMap map, Color generatorFillColor, IRandomizer randomizer);
    }
}

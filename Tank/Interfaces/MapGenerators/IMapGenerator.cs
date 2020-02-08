using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;

namespace Tank.Interfaces.MapGenerators
{
    interface IMapGenerator
    {
        /// <summary>
        /// This will generate you a new map to use
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <returns></returns>
        IMap GenerateNewMap(Position size);

        /// <summary>
        /// This will generate you a new map to use
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <param name="mapTexturizer">The instance to use to texturize the map</param>
        /// <returns></returns>
        IMap GenerateNewMap(Position size, IMapTexturizer mapTexturizer);

        /// <summary>
        /// This will generate you a new map to use
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <param name="seed">The seed to use</param>
        /// <param name="mapTexturizer">The instance to use to texturize the map</param>
        /// <returns></returns>
        IMap GenerateNewMap(Position size, int seed, IMapTexturizer mapTexturizer);

        /// <summary>
        /// This will async generate you a new map to use
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <param name="seed">The seed of the map</param>
        /// <returns></returns>
        Task<IMap> AsyncGenerateNewMap(Position size, int seed, IMapTexturizer mapTexturizer);
    }
}

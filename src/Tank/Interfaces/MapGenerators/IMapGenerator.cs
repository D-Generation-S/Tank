using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.DataStructure;

namespace Tank.src.Interfaces.MapGenerators
{
    /// <summary>
    /// This interface represents a map generator
    /// </summary>
    interface IMapGenerator
    {
        Color MapColor { get; }

        /// <summary>
        /// This method will allow you to set a map color as you want
        /// </summary>
        /// <param name="newMapColor"></param>
        void SetMapColor(Color newMapColor);

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
        /// <param name="mapTexturizer">The instance to use to texturize the map</param>
        /// <param name="seed">The seed to use</param>
        /// <returns></returns>
        IMap GenerateNewMap(Position size, IMapTexturizer mapTexturizer, int seed);

        /// <summary>
        /// This will async generate you a new map to use
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <returns></returns>
        Task<IMap> AsyncGenerateNewMap(Position size);

        /// <summary>
        /// This will async generate you a new map to use
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <param name="mapTexturizer">The texturizer to use</param>
        /// <returns></returns>
        Task<IMap> AsyncGenerateNewMap(Position size, IMapTexturizer mapTexturizer);

        /// <summary>
        /// This will async generate you a new map to use
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <param name="mapTexturizer">The texturizer to use</param>
        /// <param name="seed">The seed of the map</param>
        /// <returns></returns>
        Task<IMap> AsyncGenerateNewMap(Position size, IMapTexturizer mapTexturizer, int seed);
    }
}

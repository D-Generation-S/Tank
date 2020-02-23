using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.MapGenerators;

namespace Tank.src.Components
{
    /// <summary>
    /// This class will make an entity to an map component
    /// </summary>
    class MapComponent : BaseComponent
    {
        /// <summary>
        /// An instance of a map to use for the game
        /// </summary>
        private IMap map;

        /// <summary>
        /// Public readonly access to the map instance
        /// </summary>
        public IMap Map => map;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="map">The map instance to use</param>
        public MapComponent(IMap map)
        {
            this.map = map;
        }
    }
}

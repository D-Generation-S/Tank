using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.DataStructure;
using Tank.Interfaces.MapGenerators;

namespace Tank.Components
{
    /// <summary>
    /// This class will make an entity to an map component
    /// </summary>
    class MapComponent : BaseComponent
    {
        /// <summary>
        /// Public readonly access to the map instance
        /// </summary>
        [Obsolete]
        public IMap Map { get; set; }

        /// <summary>
        /// The seed of the map
        /// </summary>
        public int Seed;

        /// <summary>
        /// The height of the map
        /// </summary>
        public int Height;

        /// <summary>
        /// The width of the map
        /// </summary>
        public int Width;

        /// <summary>
        /// The heighest point of the map
        /// </summary>
        public float HighestPoint;

        /// <summary>
        /// Current used image data
        /// </summary>
        public FlattenArray<Color> ImageData;

        /// <summary>
        /// Image data to use for next frame
        /// </summary>
        public FlattenArray<Color> ChangedImageData;

        /// <summary>
        /// All the not solid colors of the map
        /// </summary>
        public HashSet<Color> NotSolidColors;

        /// <inheritdoc/>
        public override void Init()
        {
            Map = null;
            Seed = 0;
            Height = 0; 
            Width = 0;
            HighestPoint = 0;
            ImageData = null;
            ChangedImageData = null;
            NotSolidColors = new HashSet<Color>();
        }
    }
}

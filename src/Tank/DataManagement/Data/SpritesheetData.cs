using System.Collections.Generic;
using TankEngine.DataStructures.Serializeable;
using TankEngine.DataStructures.Spritesheet;

namespace Tank.DataManagement.Data
{
    /// <summary>
    /// Loadable dataset for spritesheet data
    /// </summary>
    class SpritesheetData
    {
        /// <summary>
        /// The name of the texture to load
        /// </summary>
        public string TextureName { get; set; }

        /// <summary>
        /// The single image size to use
        /// </summary>
        public SPoint SingleImageSize { get; set; }

        /// <summary>
        /// The distance between the images
        /// </summary>
        public int DistanceBetweenImages { get; set; }

        /// <summary>
        /// The patterns to apply for the sheet
        /// </summary>
        public List<SpriteSheetPattern> Patterns { get; set; }
    }
}

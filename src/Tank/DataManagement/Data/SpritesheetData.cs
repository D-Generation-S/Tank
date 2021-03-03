using System.Collections.Generic;
using Tank.DataStructure;
using Tank.DataStructure.Spritesheet;

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
        public string TextureName;

        /// <summary>
        /// The size of a single texture in the sheet
        /// </summary>
        public Position SingleImageSize;

        /// <summary>
        /// The distance between the images
        /// </summary>
        public int DistanceBetweenImages;

        /// <summary>
        /// The patterns to apply for the sheet
        /// </summary>
        public List<SpriteSheetPattern> Patterns;
    }
}

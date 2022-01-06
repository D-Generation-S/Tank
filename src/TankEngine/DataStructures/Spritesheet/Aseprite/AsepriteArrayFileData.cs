using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TankEngine.DataStructures.Spritesheet.Aseprite.Frames;
using TankEngine.DataStructures.Spritesheet.Aseprite.Meta;

namespace TankEngine.DataStructures.Spritesheet.Aseprite
{
    /// <summary>
    /// Class to respresent the whole asesprite file data
    /// </summary>
    public class AsepriteArrayFileData
    {
        /// <summary>
        /// The meta information for the file data
        /// </summary>
        [JsonPropertyName("meta")]
        public AsepriteMetaData Meta { get; set; }

        /// <summary>
        /// All the frames for this spritesheet
        /// </summary>
        [JsonPropertyName("frames")]
        public List<AsepriteFrame> Frames { get; set; }

        /// <summary>
        /// Get the matching aseprite spritesheet
        /// </summary>
        /// <returns>A useable spritesheet data set</returns>
        public AsepriteSpritesheet GetSpritesheet()
        {
            return new AsepriteSpritesheet(this);
        }

        /// <summary>
        /// Get the matching aseprite spritesheet
        /// </summary>
        /// <param name="dataToPropertyConversion">Conversion method to use for converting slices data to spritesheet property</param>
        /// <returns>A useable spritesheet data set</returns>
        public AsepriteSpritesheet GetSpritesheet(Func<string, List<SpritesheetProperty>> dataToPropertyConversion)
        {
            return new AsepriteSpritesheet(this, dataToPropertyConversion);
        }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;
using TankEngine.DataStructures.Spritesheet.Aseprite.Frames;
using TankEngine.DataStructures.Spritesheet.Aseprite.Meta;

namespace TankEngine.DataStructures.Spritesheet.Aseprite
{
    public class AsepriteArrayFileData
    {
        [JsonPropertyName("meta")]
        public AsepriteMetaData Meta { get; set; }

        [JsonPropertyName("frames")]
        public List<AsepriteFrame> Frames { get; set; }

        /// <summary>
        /// Get the matching aseprite spritesheet
        /// </summary>
        /// <returns></returns>
        public AsepriteSpritesheet GetSpritesheet()
        {
            return new AsepriteSpritesheet(this);
        }
    }
}

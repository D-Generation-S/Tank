using System.Text.Json.Serialization;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Meta
{
    public class AsepriteTag
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("from")]
        public int From { get; set; }

        [JsonPropertyName("to")]
        public int To { get; set; }

        [JsonPropertyName("direction")]
        public string Direction { get; set; }

        /// <summary>
        /// Convert the tag to a spritesheet frame tag
        /// </summary>
        /// <returns>The spritesheet tag to use</returns>
        public SpritesheetFrameTag GetSpritesheetFrameTag()
        {
            return new SpritesheetFrameTag(Name, From, To);
        }
    }
}

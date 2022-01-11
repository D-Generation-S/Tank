using System.Text.Json.Serialization;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Meta
{
    /// <summary>
    /// Aseprite tag for tagged frame animation
    /// </summary>
    public class AsepriteTag
    {
        /// <summary>
        /// The name of the tagged animation
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The index of the frame the tagged animation starts from
        /// </summary>
        [JsonPropertyName("from")]
        public int From { get; set; }

        /// <summary>
        /// The index of the frame the tagged animation ends
        /// </summary>
        [JsonPropertyName("to")]
        public int To { get; set; }

        /// <summary>
        /// The direction of the animation
        /// </summary>
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

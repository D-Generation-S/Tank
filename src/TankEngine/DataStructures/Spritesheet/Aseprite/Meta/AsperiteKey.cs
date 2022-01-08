using System.Text.Json.Serialization;
using TankEngine.DataStructures.Serializeable;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Meta
{
    /// <summary>
    /// A single aseprite key for a slice
    /// </summary>
    public class AsperiteKey
    {
        /// <summary>
        /// The index of the this key belongs to
        /// </summary>
        [JsonPropertyName("frame")]
        public int Frame { get; set; }

        /// <summary>
        /// The bounderies of the marked key
        /// </summary>
        [JsonPropertyName("bounds")]
        public SRectangle Bounds { get; set; }
    }
}
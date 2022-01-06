using System.Text.Json.Serialization;
using TankEngine.DataStructures.Serializeable;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Meta
{
    public class AsperiteKey
    {
        [JsonPropertyName("frame")]
        public int Frame { get; set; }

        [JsonPropertyName("bounds")]
        public SRectangle Bounds { get; set; }
    }
}
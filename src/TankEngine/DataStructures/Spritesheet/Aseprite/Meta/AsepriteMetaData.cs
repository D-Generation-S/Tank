using System.Collections.Generic;
using System.Text.Json.Serialization;
using TankEngine.DataStructures.Serializeable;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Meta
{
    public class AsepriteMetaData
    {
        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("size")]
        public SDimension Size { get; set; }

        [JsonPropertyName("scale")]
        public string Scale { get; set; }

        [JsonPropertyName("frameTags")]
        public List<AsepriteTag> FrameTags { get; set; }

        [JsonPropertyName("layers")]
        public List<AsperiteLayer> Layers { get; set; }

        [JsonPropertyName("slices")]
        public List<AsepriteSlice> Slices { get; set; }

    }
}

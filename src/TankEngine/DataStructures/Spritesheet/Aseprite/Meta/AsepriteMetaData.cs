using System.Collections.Generic;
using System.Text.Json.Serialization;
using TankEngine.DataStructures.Serializeable;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Meta
{
    /// <summary>
    /// Meta data of the aseprite json data set
    /// </summary>
    public class AsepriteMetaData
    {
        /// <summary>
        /// The name of the app the data got created with
        /// </summary>
        [JsonPropertyName("app")]
        public string App { get; set; }

        /// <summary>
        /// The version of the app the data got created with
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        /// <summary>
        /// The name of the image the data set belongs to
        /// </summary>
        [JsonPropertyName("image")]
        public string Image { get; set; }

        /// <summary>
        /// The color format of the image file
        /// </summary>
        [JsonPropertyName("format")]
        public string Format { get; set; }

        /// <summary>
        /// The size of the whole image
        /// </summary>
        [JsonPropertyName("size")]
        public SDimension Size { get; set; }

        /// <summary>
        /// The scaling of the image
        /// </summary>
        [JsonPropertyName("scale")]
        public string Scale { get; set; }

        /// <summary>
        /// All the frame tags transfered from the software
        /// </summary>
        [JsonPropertyName("frameTags")]
        public List<AsepriteTag> FrameTags { get; set; }

        /// <summary>
        /// All the layers in the image and there properties
        /// </summary>
        [JsonPropertyName("layers")]
        public List<AsperiteLayer> Layers { get; set; }

        /// <summary>
        /// All the slices markt by aseprite
        /// </summary>
        [JsonPropertyName("slices")]
        public List<AsepriteSlice> Slices { get; set; }

    }
}

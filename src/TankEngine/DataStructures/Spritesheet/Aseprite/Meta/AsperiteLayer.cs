using System;
using System.Text.Json.Serialization;
using TankEngine.DataStructures.Spritesheet.Aseprite.Enums;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Meta
{
    /// <summary>
    /// A layer of the aseprite file definition
    /// </summary>
    public class AsperiteLayer
    {
        /// <summary>
        /// The name of the layer
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The opacity of the layer
        /// </summary>
        [JsonPropertyName("opacity")]
        public int Opacity { get; set; }

        /// <summary>
        /// The blend mode of the layer
        /// </summary>
        [JsonPropertyName("blendMode")]
        public string BlendMode { get; set; }

        /// <summary>
        /// Get the Blend Mode for the layer
        /// </summary>
        /// <returns>The blend mode as a enum</returns>
        public AsepriteBlendModeEnum GetBlendMode()
        {
            AsepriteBlendModeEnum blendMode = AsepriteBlendModeEnum.Unknown;
            try
            {
                blendMode = (AsepriteBlendModeEnum)Enum.Parse(typeof(AsepriteBlendModeEnum), BlendMode);
            }
            catch (Exception)
            {
            }

            return blendMode;
        }
    }
}

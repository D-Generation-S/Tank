using System.Text.Json.Serialization;
using TankEngine.DataStructures.Serializeable;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Frames
{
    /// <summary>
    /// A aseprite frame description
    /// </summary>
    public class AsepriteFrame
    {
        /// <summary>
        /// The filename the frame belongs to
        /// </summary>
        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        /// <summary>
        /// The area of the frame on the spritesheet
        /// </summary>
        [JsonPropertyName("frame")]
        public SRectangle Frame { get; set; }

        /// <summary>
        /// Is the frame rotated
        /// </summary>
        [JsonPropertyName("rotated")]
        public bool Rotated { get; set; }

        /// <summary>
        /// Is the frame trimmed
        /// </summary>
        [JsonPropertyName("trimmed")]
        public bool Trimmed { get; set; }

        /// <summary>
        /// The source size of the frame
        /// </summary>
        [JsonPropertyName("spriteSourceSize")]
        public SRectangle SpriteSourceSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("sourceSize")]
        public SDimension SourceSize { get; set; }

        /// <summary>
        /// The duration in milliseconds for the frame animation
        /// </summary>
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Get the frame as a spritesheet frame
        /// </summary>
        /// <returns>A useable spritesheet frame</returns>
        public SpritesheetFrame GetSpritesheetFrame()
        {
            return new SpritesheetFrame(Frame, SpriteSourceSize, SourceSize, Duration);
        }
    }
}

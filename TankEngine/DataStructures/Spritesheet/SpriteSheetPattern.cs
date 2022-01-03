using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;
using TankEngine.DataStructures.Serializeable;

namespace TankEngine.DataStructures.Spritesheet
{
    /// <summary>
    /// Sprite sheet pattern do name parts of the sheet
    /// </summary>
    public class SpriteSheetPattern
    {
        /// <summary>
        /// The name of the pattern
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The position on the sheet
        /// </summary>
        public SPoint Position { get; set; }

        /// <summary>
        /// The overwritten pattern size
        /// </summary>
        public SPoint PatternSizeOverwrite { get; set; }

        /// <summary>
        /// Is the pattern size overwritten
        /// </summary>
        [JsonIgnore]
        public bool SizeOverwritten => PatternSizeOverwrite != null;

        public SpriteSheetPattern() : this(string.Empty, new Point(), null)
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="name">The name of the pattern</param>
        /// <param name="position">The position on the sheet</param>
        /// <param name="patternSizeOverwrite">The pattern size to overwrite, use null if not required</param>
        public SpriteSheetPattern(string name, Point position, Point? patternSizeOverwrite)
        {
            Name = name;
            Position = new SPoint(position);
            if (patternSizeOverwrite != null)
            {
                PatternSizeOverwrite = new SPoint(patternSizeOverwrite);
            }

        }
    }
}

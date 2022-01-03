using Microsoft.Xna.Framework;

namespace Tank.DataStructure.Spritesheet
{
    /// <summary>
    /// Sprite sheet pattern do name parts of the sheet
    /// </summary>
    public class SpriteSheetPattern
    {
        /// <summary>
        /// The name of the pattern
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The position on the sheet
        /// </summary>
        public Point Position { get; }

        /// <summary>
        /// The overwritten pattern size
        /// </summary>
        public Point PatternSizeOverwrite { get; }

        /// <summary>
        /// Is the pattern size overwritten
        /// </summary>
        public bool SizeOverwritten => PatternSizeOverwrite != null;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="name">The name of the pattern</param>
        /// <param name="position">The position on the sheet</param>
        /// <param name="patternSizeOverwrite">The pattern size to overwrite, use null if not required</param>
        public SpriteSheetPattern(string name, Point position, Point patternSizeOverwrite)
        {
            Name = name;
            Position = position;
            PatternSizeOverwrite = patternSizeOverwrite;
        }
    }
}

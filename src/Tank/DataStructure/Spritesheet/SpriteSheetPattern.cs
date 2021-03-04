namespace Tank.DataStructure.Spritesheet
{
    /// <summary>
    /// Sprite sheet pattern do name parts of the sheet
    /// </summary>
    class SpriteSheetPattern
    {
        /// <summary>
        /// The name of the pattern
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The position on the sheet
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="name">The name of the pattern</param>
        /// <param name="position">The position on the sheet</param>
        public SpriteSheetPattern(string name, Position position)
        {
            Name = name;
            Position = position;
        }
    }
}

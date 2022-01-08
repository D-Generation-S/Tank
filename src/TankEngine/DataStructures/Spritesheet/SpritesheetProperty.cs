namespace TankEngine.DataStructures.Spritesheet
{
    /// <summary>
    /// A property assigned to the spritesheet
    /// </summary>
    public class SpritesheetProperty
    {
        /// <summary>
        /// The name of the property
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The value of the property
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Create a new instance of the property class
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="value">The value of the property</param>
        public SpritesheetProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}

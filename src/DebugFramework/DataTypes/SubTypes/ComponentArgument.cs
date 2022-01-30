using System.Text.Json.Serialization;

namespace DebugFramework.DataTypes.SubTypes
{
    /// <summary>
    /// Class to describe a single argument of a component
    /// </summary>
    public class ComponentArgument : BaseDataType
    {
        /// <summary>
        /// The name of the argument
        /// </summary>
        [JsonPropertyName("key")]
        public string Name { get; set; }

        /// <summary>
        /// The value of the argument
        /// </summary>

        [JsonPropertyName("val")]
        public string Value { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public ComponentArgument()
        {
            Name = string.Empty;
            Value = string.Empty;
        }
    }
}

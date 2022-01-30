using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DebugFramework.DataTypes.SubTypes
{
    /// <summary>
    /// Class which describes a single component of an entity
    /// </summary>
    public class ComponentData : BaseDataType
    {
        /// <summary>
        /// The type of the component this belongs to
        /// </summary>
        [JsonPropertyName("type")]
        public string ComponentType { get; set; }

        /// <summary>
        /// The arguments of the component
        /// </summary>
        [JsonPropertyName("args")]
        public List<ComponentArgument> Arguments { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public ComponentData()
        {
            ComponentType = string.Empty;
            Arguments = new List<ComponentArgument>();
        }
    }
}

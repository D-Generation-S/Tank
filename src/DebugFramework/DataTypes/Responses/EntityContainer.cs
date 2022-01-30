using DebugFramework.DataTypes.SubTypes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DebugFramework.DataTypes
{
    /// <summary>
    /// Container class for a single entity from the game engine
    /// </summary>
    public class EntityContainer : BaseDataType
    {
        /// <summary>
        /// The id of the entity this container is connected to
        /// </summary>
        [JsonPropertyName("id")]
        public uint EntityId { get; set; }

        /// <summary>
        /// The components which are assigned to the entity
        /// </summary>
        [JsonPropertyName("comp")]
        public List<ComponentData> EntityComponents { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public EntityContainer()
        {
            EntityComponents = new List<ComponentData>();
        }
    }
}

using DebugFramework.DataTypes.SubTypes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DebugFramework.DataTypes
{
    public class EntityContainer : BaseDataType
    {
        [JsonPropertyName("id")]
        public uint EntityId { get; set; }

        [JsonPropertyName("comp")]
        public List<ComponentData> EntityComponents { get; set; }

        public EntityContainer()
        {
            EntityComponents = new List<ComponentData>();
        }
    }
}

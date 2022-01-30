using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DebugFramework.DataTypes.SubTypes
{
    public class ComponentData : BaseDataType
    {
        [JsonPropertyName("type")]
        public string ComponentType { get; set; }

        [JsonPropertyName("args")]
        public List<ComponentArgument> Arguments { get; set; }

        public ComponentData() : this(string.Empty)
        {
        }

        public ComponentData(string componentType)
        {
            ComponentType = componentType;
            Arguments = new List<ComponentArgument>();
        }
    }
}

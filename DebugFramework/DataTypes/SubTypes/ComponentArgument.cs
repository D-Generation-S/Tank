using System.Text.Json.Serialization;

namespace DebugFramework.DataTypes.SubTypes
{
    public class ComponentArgument : BaseDataType
    {
        [JsonPropertyName("key")]
        public string Name { get; set; }

        [JsonPropertyName("val")]
        public string Value { get; set; }

        public ComponentArgument() : this(string.Empty, string.Empty)
        {
        }

        public ComponentArgument(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}

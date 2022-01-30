using System.Collections.Generic;

namespace DebugFramework.DataTypes.SubTypes
{
    public class ComponentData : BaseDataType
    {
        public string ComponentType { get; set; }

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

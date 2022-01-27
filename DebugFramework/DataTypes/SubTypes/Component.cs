using System.Collections.Generic;

namespace DebugFramework.DataTypes.SubTypes
{
    public class Component : BaseDataType
    {
        public string ComponentType { get; set; }

        public List<ComponentArgument> Arguments { get; set; }

        public Component() : this(string.Empty)
        {
        }

        public Component(string componentType)
        {
            ComponentType = componentType;
            Arguments = new List<ComponentArgument>();
        }
    }
}

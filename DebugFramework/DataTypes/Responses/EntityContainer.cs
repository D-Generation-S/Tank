using DebugFramework.DataTypes.SubTypes;
using System.Collections.Generic;

namespace DebugFramework.DataTypes
{
    public class EntityContainer : BaseDataType
    {
        public uint EntityId { get; set; }

        public List<Component> EntityComponents { get; set; }

        public EntityContainer()
        {
            EntityComponents = new List<Component>();
        }
    }
}

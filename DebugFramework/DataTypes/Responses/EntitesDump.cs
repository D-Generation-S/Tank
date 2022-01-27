using System.Collections.Generic;

namespace DebugFramework.DataTypes
{
    public class EntitesDump : BaseDataType
    {
        public List<EntityContainer> Entites { get; set; }

        public EntitesDump()
        {
            Entites = new List<EntityContainer>();
        }
    }
}

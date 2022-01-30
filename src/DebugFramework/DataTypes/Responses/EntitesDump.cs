using System.Collections.Generic;

namespace DebugFramework.DataTypes
{
    /// <summary>
    /// A dump of all the entites which are in the game right now
    /// </summary>
    public class EntitesDump : BaseDataType
    {
        /// <summary>
        /// List which contains all entites in the game
        /// </summary>
        public List<EntityContainer> Entites { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public EntitesDump()
        {
            Entites = new List<EntityContainer>();
        }
    }
}

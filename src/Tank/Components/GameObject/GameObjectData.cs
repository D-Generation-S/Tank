using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Components.GameObject
{
    class GameObjectData : BaseComponent
    {
        /// <summary>
        /// All the properties for this game object
        /// </summary>
        public Dictionary<string, float> Properties;

        /// <summary>
        /// Did any data got changed
        /// </summary>
        public bool DataChanged;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public GameObjectData()
        {
            Properties = new Dictionary<string, float>();
        }

        /// <inheritdoc/>
        public override void Init()
        {
            if (Properties != null)
            {
                Properties.Clear();
            }
            DataChanged = false;
        }
    }
}

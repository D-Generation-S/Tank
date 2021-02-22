using System;
using Tank.DataStructure;

namespace Tank.Events.TerrainEvents
{
    /// <summary>
    /// This class is an event telling the system there should be some terrain damage
    /// </summary>
    class DamageTerrainEvent : EventArgs
    {
        /// <summary>
        /// The area where the damage should be applied to
        /// </summary>
        private readonly Circle damageArea;

        /// <summary>
        /// Readonly access to the area where the damage should be applied to
        /// </summary>
        public Circle DamageArea => damageArea;

        /// <summary>
        /// Create a new instance of this event
        /// </summary>
        /// <param name="damageArea"></param>
        public DamageTerrainEvent(Circle damageArea)
        {
            this.damageArea = damageArea;
        }
    }
}

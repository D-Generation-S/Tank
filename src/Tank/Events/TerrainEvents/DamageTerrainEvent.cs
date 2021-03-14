using System;
using Tank.DataStructure.Geometrics;

namespace Tank.Events.TerrainEvents
{
    /// <summary>
    /// This class is an event telling the system there should be some terrain damage
    /// </summary>
    class DamageTerrainEvent : IGameEvent
    {
        /// <summary>
        /// Readonly access to the area where the damage should be applied to
        /// </summary>
        public Circle DamageArea;

        public Type Type { get; }

        public DamageTerrainEvent()
        {
            Type = this.GetType();
        }

        public void Init()
        {
            DamageArea = null;            
        }
    }
}

﻿using TankEngine.DataStructures.Geometrics;
using TankEngine.EntityComponentSystem.Events;

namespace Tank.Events.TerrainEvents
{
    /// <summary>
    /// This class is an event telling the system there should be some terrain damage
    /// </summary>
    class DamageTerrainEvent : BaseEvent
    {
        /// <summary>
        /// Readonly access to the area where the damage should be applied to
        /// </summary>
        public Circle DamageArea;

        /// <inheritdoc/>
        public override void Init()
        {
            DamageArea = null;
        }
    }
}

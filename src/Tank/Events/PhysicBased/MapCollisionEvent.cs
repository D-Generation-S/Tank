using Microsoft.Xna.Framework;

namespace Tank.Events.PhysicBased
{
    /// <summary>
    /// This event will describe an collision with the terrain
    /// </summary>
    class MapCollisionEvent : EntityBasedEvent
    {
        /// <summary>
        /// Readonly access to the collision position
        /// </summary>
        public Vector2 Position;

        /// <inheritdoc/>
        public override void Init()
        {
            base.Init();
            Position = Vector2.Zero;
        }
    }
}

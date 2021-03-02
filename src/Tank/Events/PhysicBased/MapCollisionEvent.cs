using Microsoft.Xna.Framework;

namespace Tank.Events.PhysicBased
{
    /// <summary>
    /// This event will describe an collision with the terrain
    /// </summary>
    class MapCollisionEvent : EntityBasedEvent
    {
        /// <summary>
        /// The position of the collision
        /// </summary>
        private readonly Vector2 position;

        /// <summary>
        /// Readonly access to the collision position
        /// </summary>
        public Vector2 Position => position;

        /// <summary>
        /// Create a new instance of the class
        /// </summary>
        /// <param name="entityId">The id of the entity which did collide</param>
        /// <param name="collisionPosition">The position of the collision</param>
        public MapCollisionEvent(uint entityId, Vector2 collisionPosition) : base(entityId)
        {
            this.position = collisionPosition;
        }
    }
}

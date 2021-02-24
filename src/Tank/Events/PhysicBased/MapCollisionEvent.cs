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
        private readonly Vector2 collisionPosition;

        /// <summary>
        /// Readonly access to the collision position
        /// </summary>
        public Vector2 CollisionPosition => collisionPosition;

        /// <summary>
        /// Create a new instance of the class
        /// </summary>
        /// <param name="entityId">The id of the entity which did collide</param>
        /// <param name="collisionPosition">The position of the collision</param>
        public MapCollisionEvent(uint entityId, Vector2 collisionPosition) : base(entityId)
        {
            this.collisionPosition = collisionPosition;
        }
    }
}

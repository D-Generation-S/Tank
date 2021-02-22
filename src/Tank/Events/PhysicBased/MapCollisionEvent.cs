using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.DataStructure;

namespace Tank.src.Events.PhysicBased
{
    /// <summary>
    /// This event will describe an collision with the terrain
    /// </summary>
    class MapCollisionEvent : EntityBasedEvent
    {
        /// <summary>
        /// The position of the collision
        /// </summary>
        private readonly Position collisionPosition;

        /// <summary>
        /// Readonly access to the collision position
        /// </summary>
        public Position CollisionPosition => collisionPosition;

        /// <summary>
        /// Create a new instance of the class
        /// </summary>
        /// <param name="entityId">The id of the entity which did collide</param>
        /// <param name="collisionPosition">The position of the collision</param>
        public MapCollisionEvent(uint entityId, Vector2 collisionPosition)
            : this(entityId, new Position((int)collisionPosition.X, (int)collisionPosition.Y))
        {
        }

        /// <summary>
        /// Create a new instance of the class
        /// </summary>
        /// <param name="entityId">The id of the entity which did collide</param>
        /// <param name="x">The x position of the collision</param>
        /// <param name="y">The y position of the collision</param>
        public MapCollisionEvent(uint entityId, int x, int y)
            : this(entityId, new Position(x, y))
        {
        }

        /// <summary>
        /// Create a new instance of the class
        /// </summary>
        /// <param name="entityId">The id of the entity which did collide</param>
        /// <param name="collisionPosition">The position of the collision</param>
        public MapCollisionEvent(uint entityId, Position collisionPosition)
            : base(entityId)
        {
            this.collisionPosition = collisionPosition;
        }
    }
}

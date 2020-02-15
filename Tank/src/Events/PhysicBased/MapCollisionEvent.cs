using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;

namespace Tank.src.Events.PhysicBased
{
    class MapCollisionEvent : EntityBasedEvent
    {
        private readonly Position collisionPosition;
        public Position CollisionPosition => collisionPosition;

        public MapCollisionEvent(uint entityId, Vector2 collisionPosition)
            : this(entityId, new Position((int)collisionPosition.X, (int)collisionPosition.Y))
        {
        }

        public MapCollisionEvent(uint entityId, int x, int y)
            : this(entityId, new Position(x, y))
        {
        }

        public MapCollisionEvent(uint entityId, Position collisionPosition)
            : base(entityId)
        {
            this.collisionPosition = collisionPosition;
        }
    }
}

using Microsoft.Xna.Framework;

namespace Tank.Components
{
    /// <summary>
    /// This component describes a collider for an entity
    /// </summary>
    class ColliderComponent : BaseComponent
    {
        /// <summary>
        /// Public readonly access if the entity can collide with the map
        /// </summary>
        public bool MapCollision { get; }

        /// <summary>
        /// Public access to the collider rectangle
        /// </summary>
        public Rectangle Collider { get; set; }

        public bool FireCollideEvent { get; set;  }

        /// <summary>
        /// Create a new instance of this class. Map collision is on
        /// </summary>
        public ColliderComponent()
            : this(true)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="mapCollision">Should be entity collide with the map</param>
        public ColliderComponent(bool mapCollision)
            : this(mapCollision, false)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="mapCollision">Should be entity collide with the map</param>
        public ColliderComponent(bool mapCollision, bool fireCollideEvent)
        {
            MapCollision = mapCollision;
            FireCollideEvent = fireCollideEvent;
        }

    }
}

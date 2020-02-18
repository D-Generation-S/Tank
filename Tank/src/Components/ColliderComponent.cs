using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Components
{
    /// <summary>
    /// This component describes a collider for an entity
    /// </summary>
    class ColliderComponent : BaseComponent
    {
        /// <summary>
        /// The entity can collide with the map
        /// </summary>
        private bool mapCollision;

        /// <summary>
        /// Public readonly access if the entity can collide with the map
        /// </summary>
        public bool MapCollision => mapCollision;

        /// <summary>
        /// The collider rectanble
        /// </summary>
        private Rectangle collider;

        /// <summary>
        /// Public access to the collider rectangle
        /// </summary>
        public Rectangle Collider
        {
            get => collider;
            set => collider = value;
        }

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
        {
            this.mapCollision = mapCollision;
        }
    }
}

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
        public bool MapCollision { get; set; }

        /// <summary>
        /// Public access to the collider rectangle
        /// </summary>
        public Rectangle Collider { get; set; }

        /// <summary>
        /// Fire a collide event
        /// </summary>
        public bool FireCollideEvent { get; set; }

        /// <summary>
        /// Fire the event always below
        /// </summary>
        public bool FireBelow { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            MapCollision = false;
            FireCollideEvent = false;
            FireBelow = false;
            Collider = Rectangle.Empty;
        }
    }
}

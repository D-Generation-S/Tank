using Microsoft.Xna.Framework;
using System;
using TankEngine.EntityComponentSystem.Components;

namespace Tank.Components
{
    /// <summary>
    /// Allow the entity to be placed in the world
    /// </summary>
    [Obsolete("Use the Position Component from the core engine")]
    class PlaceableComponent : BaseComponent
    {
        /// <summary>
        /// Public access to the position of the entity in the world
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Public access to the rotation of the entity
        /// </summary>
        public float Rotation { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            Position = Vector2.One * 10 * -1;
            Rotation = 0;
        }
    }
}

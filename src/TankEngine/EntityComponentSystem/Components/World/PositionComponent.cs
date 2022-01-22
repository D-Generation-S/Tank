using Microsoft.Xna.Framework;

namespace TankEngine.EntityComponentSystem.Components.World
{
    /// <summary>
    /// Component to place an entity in the game world
    /// </summary>
    public class PositionComponent : BaseComponent
    {
        /// <summary>
        /// The position of the entity
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The rotation of the entity
        /// </summary>
        public float Rotation { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            Position = Vector2.Zero;
            Rotation = 0;
        }
    }
}

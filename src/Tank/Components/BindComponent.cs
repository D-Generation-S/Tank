using Microsoft.Xna.Framework;

namespace Tank.Components
{
    /// <summary>
    /// Component used to bind object together
    /// </summary>
    class BindComponent : BaseComponent
    {
        /// <summary>
        /// The entity this is bound to
        /// </summary>
        public uint BoundEntityId { get; set; }

        /// <summary>
        /// The offset this entity is bound to
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Is this entity the source of the binding
        /// </summary>
        public bool Source { get; set; }

        /// <summary>
        /// Is this entity position bound
        /// </summary>
        public bool PositionBound { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            BoundEntityId = 0;
            Offset = Vector2.Zero;
            Source = false;
            PositionBound = false;
        }
    }
}

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
        private uint boundEntityID;

        /// <summary>
        /// The entity this is bound to
        /// </summary>
        public uint BoundEntityId
        {
            get => boundEntityID;
            set => boundEntityID = value;
        }

        /// <summary>
        /// The offset this entity is bound to
        /// </summary>
        private Vector2 offset;

        /// <summary>
        /// The offset this entity is bound to
        /// </summary>
        public Vector2 Offset
        {
            get => offset;
            set => offset = value;
        }

        /// <summary>
        /// Is this entity the source of the binding
        /// </summary>
        private bool source;

        /// <summary>
        /// Is this entity the source of the binding
        /// </summary>
        public bool Source
        {
            get => source;
            set => source = value;
        }

        /// <summary>
        /// Is this entity position bound
        /// </summary>
        private bool positionBound;

        /// <summary>
        /// Is this entity position bound
        /// </summary>
        public bool PositionBound
        {
            get => positionBound;
            set => positionBound = value;
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public BindComponent() : this(0)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="boundEntityID">The entity this is bound to</param>
        public BindComponent(uint boundEntityID) : this(boundEntityID, Vector2.Zero)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="boundEntityID">The entity this is bound to</param>
        /// <param name="offset">The offset to the bound entity</param>
        public BindComponent(uint boundEntityID, Vector2 offset) : this(boundEntityID, offset, false)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="boundEntityID">The entity this is bound to</param>
        /// <param name="offset">The offset to the bound entity</param>
        /// <param name="source">Is this the source entity for binding</param>
        public BindComponent(uint boundEntityID, Vector2 offset, bool source) : this(boundEntityID, offset, source, false)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="boundEntityID">The entity this is bound to</param>
        /// <param name="offset">The offset to the bound entity</param>
        /// <param name="source">Is this the source entity for binding</param>
        /// <param name="positionBound">Should the position be vound as well</param>
        public BindComponent(uint boundEntityID, Vector2 offset, bool source, bool positionBound)
        {
            this.boundEntityID = boundEntityID;
            this.offset = offset;
            this.source = source;
            this.positionBound = positionBound;
        }
    }
}

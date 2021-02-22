using Microsoft.Xna.Framework;

namespace Tank.Components
{
    class BindComponent : BaseComponent
    {
        private uint boundEntityID;

        public uint BoundEntityId
        {
            get => boundEntityID;
            set => boundEntityID = value;
        }

        private Vector2 offset;

        public Vector2 Offset
        {
            get => offset;
            set => offset = value;
        }

        private bool source;

        public bool Target
        {
            get => source;
            set => source = value;
        }

        private bool positionBound;

        public bool PositionBound
        {
            get => positionBound;
            set => positionBound = value;
        }

        public BindComponent() : this(0)
        {
        }

        public BindComponent(uint boundEntityID) : this(boundEntityID, Vector2.Zero)
        {
        }

        public BindComponent(uint boundEntityID, Vector2 offset) : this(boundEntityID, offset, false)
        {
        }

        public BindComponent(uint boundEntityID, Vector2 offset, bool source) : this(boundEntityID, offset, source, false)
        {
        }

        public BindComponent(uint boundEntityID, Vector2 offset, bool source, bool positionBound)
        {
            this.boundEntityID = boundEntityID;
            this.offset = offset;
            this.source = source;
            this.positionBound = positionBound;
        }
    }
}

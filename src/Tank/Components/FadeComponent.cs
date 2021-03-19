namespace Tank.Components
{
    /// <summary>
    /// A component to fade in/out some entity
    /// </summary>
    class FadeComponent : BaseComponent
    {
        /// <summary>
        /// The target opacity
        /// </summary>
        public int TargetOpacity;

        /// <summary>
        /// The start opacity
        /// </summary>
        public int StartOpacity;

        /// <summary>
        /// The change of opacity per tick
        /// </summary>
        public float OpacityChange;

        /// <summary>
        /// The real change per tick
        /// </summary>
        public float RealOpacityChange;

        /// <summary>
        /// The ticks until the object gets fade out
        /// </summary>
        public int TicksToLive;

        /// <inheritdoc/>
        public override void Init()
        {
            TargetOpacity = 100;
            StartOpacity = 0;
            OpacityChange = 0.5f;
            RealOpacityChange = 0;
            TicksToLive = 60;
        }
    }
}

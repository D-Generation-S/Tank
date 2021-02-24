using Tank.Enums;

namespace Tank.Components.Forces
{
    /// <summary>
    /// Force component
    /// </summary>
    internal class ForceComponent : BaseComponent
    {
        /// <summary>
        /// The radius of the force
        /// </summary>
        public float ForceRadius { get; set; }

        /// <summary>
        /// The strenght at the center
        /// </summary>
        public float ForceBaseStrenght { get; set; }

        /// <summary>
        /// The type of the force
        /// </summary>
        public ForceTypeEnum ForceType { get; set; }

        /// <summary>
        /// The trigger for the force
        /// </summary>
        public ForceTriggerTimeEnum ForceTrigger { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            ForceRadius = 0;
            ForceBaseStrenght = 0;
            ForceType = ForceTypeEnum.None;
            ForceTrigger = ForceTriggerTimeEnum.None;
        }
    }
}

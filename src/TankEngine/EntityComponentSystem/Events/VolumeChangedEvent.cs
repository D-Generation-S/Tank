using TankEngine.Enums;

namespace TankEngine.EntityComponentSystem.Events
{
    /// <summary>
    /// Event to change the volume
    /// </summary>
    public class VolumeChangedEvent : BaseEvent
    {
        /// <summary>
        /// The type of the volume to change
        /// </summary>
        public VolumeTypeEnum VolumeType;

        /// <summary>
        /// The new volume
        /// </summary>
        public float NewVolume;

        /// <summary>
        /// Create a new instance of this event
        /// </summary>
        /// <param name="newVolume">The new volume to use</param>
        public VolumeChangedEvent()
        {
            this.VolumeType = VolumeTypeEnum.Unknown;
            this.NewVolume = 0;
        }
    }
}

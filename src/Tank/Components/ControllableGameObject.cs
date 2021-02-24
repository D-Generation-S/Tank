namespace Tank.Components
{
    /// <summary>
    /// This class will make a game object controllable
    /// </summary>
    class ControllableGameObject : BaseComponent
    {
        /// <summary>
        /// The rotation of the barrel
        /// </summary>
        public float BarrelRotation;

        /// <summary>
        /// The strenght to fire the next round
        /// </summary>
        public float Strenght;

        /// <inheritdoc/>
        public override void Init()
        {
        }
    }
}

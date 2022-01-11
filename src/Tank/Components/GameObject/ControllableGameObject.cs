using Microsoft.Xna.Framework;
using TankEngine.EntityComponentSystem.Components;

namespace Tank.Components.GameObject
{
    /// <summary>
    /// This class will make a game object controllable
    /// </summary>
    class ControllableGameObject : BaseComponent
    {
        /// <summary>
        /// The rotation of the barrel
        /// </summary>
        public float BarrelRotationDegree;

        /// <summary>
        /// The rotation of the barrel in radians
        /// </summary>
        public float BarrelRotationRadians => MathHelper.ToRadians(BarrelRotationDegree);

        /// <summary>
        /// The strenght to fire the next round
        /// </summary>
        public float Strength;

        /// <summary>
        /// The currently selected projectile
        /// </summary>
        public int SelectedProjectile;
        //public int MaxProjectiles;

        /// <summary>
        /// The team of the player
        /// </summary>
        public int Team;

        /// <inheritdoc/>
        public override void Init()
        {
            BarrelRotationDegree = 270;
            Strength = 5;
            SelectedProjectile = 0;
        }
    }
}

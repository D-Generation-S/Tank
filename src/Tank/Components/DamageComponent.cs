using Tank.src.DataStructure;
using Tank.src.Interfaces.Factories;

namespace Tank.Components
{
    /// <summary>
    /// This component represents that it can damage other entites
    /// </summary>
    class DamageComponent : BaseComponent
    {
        /// <summary>
        /// Can the entity damage the terrain
        /// </summary>
        private readonly bool damageTerrain;

        /// <summary>
        /// Readonly access if the entity can damange the terrain
        /// </summary>
        public bool DamangeTerrain => damageTerrain;

        /// <summary>
        /// The area the damage should be applied to
        /// </summary>
        private readonly Circle damageArea;

        /// <summary>
        /// Public readonly access to the area the damage should be applied to
        /// </summary>
        public Circle DamageArea => damageArea;

        /// <summary>
        /// The builder to use to get the hit effect
        /// </summary>
        private readonly IGameObjectFactory effectFactory;

        /// <summary>
        /// Public access to the effect factory
        /// </summary>
        public IGameObjectFactory EffectFactory => effectFactory;

        /// <summary>
        /// Is this an explosive damange
        /// </summary>
        private readonly bool effect;

        /// <summary>
        /// Public access to the bool if this is a explosive
        /// </summary>
        public bool Effect => effect;

        /// <summary>
        /// Was the damage already applied
        /// </summary>
        private bool damagingDone;

        /// <summary>
        /// Public access if the damage was already applied
        /// </summary>
        public bool DamagingDone
        {
            get => damagingDone;
            set => damagingDone = value;
        }

        /// <summary>
        /// The damage in the center of the damage area
        /// </summary>
        private readonly int centerDamageValue;

        /// <summary>
        /// Readonly access to the damange in the center of the damage area
        /// </summary>
        public int CenterDamageValue => centerDamageValue;

        private readonly float pushbackForce;

        public float PushbackForce => pushbackForce;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="damageTerrain">Can the entity damange the terrain</param>
        /// <param name="centerDamage">´Damage for the center of the damage area</param>
        /// <param name="damageArea">The area to apply the damage to</param>
        /// <param name="effectBuilder">The builder to use for the effects</param>
        public DamageComponent(bool damageTerrain, int centerDamage, Circle damageArea, IGameObjectFactory effectBuilder)
            : this(damageTerrain, centerDamage, damageArea, effectBuilder, 0)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="damageTerrain">Can the entity damange the terrain</param>
        /// <param name="centerDamage">´Damage for the center of the damage area</param>
        /// <param name="damageArea">The area to apply the damage to</param>
        /// <param name="effectBuilder">The builder to use for the effects</param>
        public DamageComponent(bool damageTerrain, int centerDamage, Circle damageArea, IGameObjectFactory effectBuilder, float pushbackForce)
        {
            this.damageTerrain = damageTerrain;
            centerDamageValue = centerDamage;
            this.damageArea = damageArea;
            effectFactory = effectBuilder;
            effect = effectBuilder != null;
            this.pushbackForce = pushbackForce;
        }
    }
}

using Tank.DataStructure;
using Tank.Interfaces.Factories;

namespace Tank.Components
{
    /// <summary>
    /// This component represents that it can damage other entites
    /// </summary>
    class DamageComponent : BaseComponent
    {
        /// <summary>
        /// Readonly access if the entity can damange the terrain
        /// </summary>
        public bool DamangeTerrain { get; }

        /// <summary>
        /// Public readonly access to the area the damage should be applied to
        /// </summary>
        public Circle DamageArea { get; }

        /// <summary>
        /// Public access to the effect factory
        /// </summary>
        public IGameObjectFactory EffectFactory { get; }

        /// <summary>
        /// Public access to the bool if this is a explosive
        /// </summary>
        public bool Effect { get; }

        /// <summary>
        /// Public access if the damage was already applied
        /// </summary>
        public bool DamagingDone { get; set; }

        /// <summary>
        /// Readonly access to the damange in the center of the damage area
        /// </summary>
        public int CenterDamageValue { get; }

        /// <summary>
        /// The pushback to apply
        /// </summary>
        public float PushbackForce { get; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="damageTerrain">Can the entity damange the terrain</param>
        /// <param name="centerDamage">´Damage for the center of the damage area</param>
        /// <param name="damageArea">The area to apply the damage to</param>
        /// <param name="effectBuilder">The builder to use for the effects</param>
        public DamageComponent(
            bool damageTerrain,
            int centerDamage,
            Circle damageArea,
            IGameObjectFactory effectBuilder
            )
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
        public DamageComponent(
            bool damageTerrain,
            int centerDamage,
            Circle damageArea,
            IGameObjectFactory effectBuilder,
            float pushbackForce
            )
            : this (damageTerrain, centerDamage, damageArea, effectBuilder, pushbackForce, 1)
        {
        }


        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="damageTerrain">Can the entity damange the terrain</param>
        /// <param name="centerDamage">´Damage for the center of the damage area</param>
        /// <param name="damageArea">The area to apply the damage to</param>
        /// <param name="effectBuilder">The builder to use for the effects</param>
        public DamageComponent(
            bool damageTerrain,
            int centerDamage,
            Circle damageArea,
            IGameObjectFactory effectBuilder,
            float pushbackForce,
            float pushbackAreaMultiplier
            )
        {
            DamangeTerrain = damageTerrain;
            CenterDamageValue = centerDamage;
            DamageArea = damageArea;
            EffectFactory = effectBuilder;
            Effect = effectBuilder != null;
            PushbackForce = pushbackForce;
            //PushbackAreaMultiplier = pushbackAreaMultiplier;
        }
    }
}

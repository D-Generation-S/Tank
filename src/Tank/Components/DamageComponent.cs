using Tank.DataStructure.Geometrics;
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
        public bool DamageTerrain { get; set; }

        /// <summary>
        /// Public readonly access to the area the damage should be applied to
        /// </summary>
        public Circle DamageArea { get; set;  }

        /// <summary>
        /// Public access to the effect factory
        /// </summary>
        public IGameObjectFactory EffectFactory { get; set;  }

        /// <summary>
        /// Public access to the bool if this is a explosive
        /// </summary>
        public bool Effect => EffectFactory != null;

        /// <summary>
        /// Public access if the damage was already applied
        /// </summary>
        public bool DamagingDone { get; set; }

        /// <summary>
        /// Readonly access to the damange in the center of the damage area
        /// </summary>
        public int CenterDamageValue { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            DamageTerrain = false;
            DamageArea = null;
            EffectFactory = null;
            DamagingDone = false;
            CenterDamageValue = 0;
        }
    }
}

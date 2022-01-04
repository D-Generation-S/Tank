using TankEngine.EntityComponentSystem.Components.Sound;
using TankEngine.EntityComponentSystem.Manager;

namespace TankEngine.EntityComponentSystem.Validator
{
    /// <summary>
    /// This class will validate an entity as an sound effect
    /// </summary>
    class SoundEffectValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return entityManager.HasComponent(entityId, typeof(SoundEffectComponent));
        }
    }
}

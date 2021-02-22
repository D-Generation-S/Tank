using Tank.Components;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.src.EntityComponentSystem.Validator;

namespace Tank.Validator
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

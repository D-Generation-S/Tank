using Tank.Components.Rendering;
using Tank.EntityComponentSystem.Validator;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    /// <summary>
    /// This class will validate entites which can be used by the animation system 
    /// </summary>
    class AnimationEntityValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(AnimationComponent));
            valid &= entityManager.HasComponent(entityId, typeof(VisibleComponent));
            return valid;
        }
    }
}

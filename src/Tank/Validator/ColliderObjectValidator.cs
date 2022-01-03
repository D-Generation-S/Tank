using Tank.Components;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace Tank.Validator
{
    /// <summary>
    /// This class will validate all the entites which can collide
    /// </summary>
    class ColliderObjectValidator : IValidatable
    {
        /// <inheritdoc/>
        public virtual bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(PlaceableComponent));
            valid &= entityManager.HasComponent(entityId, typeof(ColliderComponent));
            return valid;
        }
    }
}

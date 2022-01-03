using Tank.Components;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace Tank.Validator
{
    /// <summary>
    /// This class will validate all the entites which makes damage to other entites
    /// </summary>
    class DamageEntityValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(PlaceableComponent));
            valid &= entityManager.HasComponent(entityId, typeof(DamageComponent));
            return valid;
        }
    }
}

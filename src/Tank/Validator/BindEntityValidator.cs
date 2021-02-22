using Tank.Components;
using Tank.EntityComponentSystem.Validator;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    /// <summary>
    /// Validator for bind entities
    /// </summary>
    class BindEntityValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(PlaceableComponent));
            valid &= entityManager.HasComponent(entityId, typeof(BindComponent));
            return valid;
        }
    }
}

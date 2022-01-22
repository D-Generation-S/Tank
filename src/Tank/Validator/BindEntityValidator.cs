using Tank.Components;
using TankEngine.EntityComponentSystem.Components.World;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

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
            bool valid = entityManager.HasComponent(entityId, typeof(PositionComponent));
            valid &= entityManager.HasComponent(entityId, typeof(BindComponent));
            return valid;
        }
    }
}

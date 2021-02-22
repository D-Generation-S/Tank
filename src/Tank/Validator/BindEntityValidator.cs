using Tank.Components;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.src.EntityComponentSystem.Validator;

namespace Tank.Validator
{
    class BindEntityValidator : IValidatable
    {
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(PlaceableComponent));
            valid &= entityManager.HasComponent(entityId, typeof(BindComponent));
            return valid;
        }
    }
}

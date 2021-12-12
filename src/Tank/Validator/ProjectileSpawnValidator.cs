using Tank.Components;
using Tank.Components.GameObject;
using Tank.EntityComponentSystem.Validator;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    class ProjectileSpawnValidator : IValidatable
    {
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent<PlaceableComponent>(entityId);
            valid &= entityManager.HasComponent<ProjectileSpawnComponent>(entityId);
            return valid;
        }
    }
}

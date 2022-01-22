using Tank.Components.GameObject;
using TankEngine.EntityComponentSystem.Components.World;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace Tank.Validator
{
    class ProjectileSpawnValidator : IValidatable
    {
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent<PositionComponent>(entityId);
            valid &= entityManager.HasComponent<ProjectileSpawnComponent>(entityId);
            return valid;
        }
    }
}

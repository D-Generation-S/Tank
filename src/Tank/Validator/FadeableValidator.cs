using Tank.Components;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    class FadeableValidator : RenderableEntityValidator
    {
        public override bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return base.IsValidEntity(entityId, entityManager) && entityManager.HasComponent<FadeComponent>(entityId);
        }
    }
}

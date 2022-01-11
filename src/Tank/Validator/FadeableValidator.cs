using Tank.Components;
using TankEngine.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    /// <summary>
    /// This class will validate any entity which will fade out or fade in
    /// </summary>
    class FadeableValidator : RenderableEntityValidator
    {
        /// <inheritdoc/>
        public override bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return base.IsValidEntity(entityId, entityManager) && entityManager.HasComponent<FadeComponent>(entityId);
        }
    }
}

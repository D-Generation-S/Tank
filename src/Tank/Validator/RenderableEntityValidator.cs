using Tank.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace Tank.Validator
{
    /// <summary>
    /// This class will validate all the entites which can be rendered
    /// </summary>
    class RenderableEntityValidator : IValidatable
    {
        /// <inheritdoc/>
        public virtual bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent<PositionComponent>(entityId);
            valid &= entityManager.HasComponent<TextureComponent>(entityId) || entityManager.HasComponent(entityId, typeof(VisibleTextComponent));
            return valid;
        }
    }
}

using Tank.Components;
using Tank.Components.Rendering;
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
            bool valid = entityManager.HasComponent(entityId, typeof(PlaceableComponent));
            valid &= entityManager.HasComponent(entityId, typeof(VisibleComponent)) || entityManager.HasComponent(entityId, typeof(VisibleTextComponent));
            return valid;
        }
    }
}

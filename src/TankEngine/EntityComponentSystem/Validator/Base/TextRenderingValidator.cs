using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;
using TankEngine.EntityComponentSystem.Manager;

namespace TankEngine.EntityComponentSystem.Validator.Base
{
    /// <summary>
    /// Validator to find entites which should render a text
    /// </summary>
    public class TextRenderingValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return entityManager.HasComponent<PositionComponent>(entityId) && entityManager.HasComponent<TextComponent>(entityId);
        }
    }
}

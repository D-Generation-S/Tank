using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;
using TankEngine.EntityComponentSystem.Manager;

namespace TankEngine.EntityComponentSystem.Validator.Base
{
    /// <summary>
    /// Validate all the texture entitites which can be rendered
    /// </summary>
    public class TextureRenderingValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return entityManager.HasComponent<PositionComponent>(entityId) && entityManager.HasComponent<TextureComponent>(entityId);
        }
    }
}

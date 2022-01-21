using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;
using TankEngine.EntityComponentSystem.Manager;

namespace TankEngine.EntityComponentSystem.Validator.Base
{
    /// <summary>
    /// Validate all enties which are camera entites
    /// </summary>
    public class CameraEntityValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return entityManager.HasComponent<CameraComponent>(entityId) && entityManager.HasComponent<PositionComponent>(entityId);
        }
    }
}

using Tank.Components;
using Tank.Components.Tags;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    /// <summary>
    /// This class will calidate all the entites which can collide with the map
    /// </summary>
    class MapColliderValidator : ColliderObjectValidator
    {
        /// <inheritdoc/>
        public override bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = base.IsValidEntity(entityId, entityManager);
            valid &= entityManager.HasComponent(entityId, typeof(MoveableComponent));
            valid &= entityManager.HasComponent(entityId, typeof(MapColliderTag));
            return valid;
        }
    }
}

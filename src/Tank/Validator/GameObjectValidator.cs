using Tank.Components.Tags;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace Tank.Validator
{
    /// <summary>
    /// This class will check if a object is a game object
    /// </summary>
    class GameObjectValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(GameObjectTag));
            return valid;
        }
    }
}

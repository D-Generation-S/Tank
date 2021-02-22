using Tank.Components.Tags;
using Tank.EntityComponentSystem.Validator;
using Tank.Interfaces.EntityComponentSystem.Manager;

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

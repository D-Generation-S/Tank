using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.src.Components.Tags;
using Tank.src.EntityComponentSystem.Validator;

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

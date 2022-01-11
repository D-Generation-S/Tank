using Tank.Components.GameObject;
using Tank.Components.Input;
using Tank.Components.Tags;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace Tank.Validator.Input
{
    class KeyboardControllableObjectValidator : IValidatable
    {
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent<KeyboardControllerComponent>(entityId);
            valid &= entityManager.HasComponent<PlayerControlledTag>(entityId);
            valid &= entityManager.HasComponent<ControllableGameObject>(entityId);
            return valid;
        }
    }
}

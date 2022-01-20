using System.Linq;
using TankEngine.EntityComponentSystem.Manager;

namespace TankEngine.EntityComponentSystem.Validator.LogicValidators
{
    /// <summary>
    /// Or connect multiple validators
    /// </summary>
    public class OrValidator : BaseLogicValidator
    {
        /// <inheritdoc/>
        public override bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return validatables.Any(validator => validator.IsValidEntity(entityId, entityManager));
        }
    }
}
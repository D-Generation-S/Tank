using System.Linq;
using TankEngine.EntityComponentSystem.Manager;

namespace TankEngine.EntityComponentSystem.Validator.LogicValidators
{
    /// <summary>
    /// And connect multiple validators
    /// </summary>
    public class AndValidator : BaseLogicValidator
    {
        /// <inheritdoc/>
        public override bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return validatables.All(validatable => validatable.IsValidEntity(entityId, entityManager));
        }
    }
}

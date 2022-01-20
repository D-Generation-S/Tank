using System.Collections.Generic;
using System.Linq;
using TankEngine.EntityComponentSystem.Manager;

namespace TankEngine.EntityComponentSystem.Validator.LogicValidators
{
    /// <summary>
    /// And connect multiple validators
    /// </summary>
    public class AndValidator : BaseLogicValidator
    {
        /// <summary>
        /// Createa a new instance of this class
        /// </summary>
        /// <param name="validators">The list of validators to use</param>
        public AndValidator(List<IValidatable> validators) : base(validators) { }

        /// <summary>
        /// Createa a new instance of this class
        /// </summary>
        /// <param name="validators">The list of validators to use</param>
        public AndValidator(params IValidatable[] validators) : base(validators) { }

        /// <inheritdoc/>
        public override bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return validatables.All(validatable => validatable.IsValidEntity(entityId, entityManager));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TankEngine.EntityComponentSystem.Manager;

namespace TankEngine.EntityComponentSystem.Validator.LogicValidators
{
    /// <summary>
    /// Or connect multiple validators
    /// </summary>
    public class OrValidator : BaseLogicValidator
    {
        /// <summary>
        /// Createa a new instance of this class
        /// </summary>
        /// <param name="validators">The list of validators to use</param>
        public OrValidator(List<IValidatable> validators) : base(validators) { }

        /// <summary>
        /// Createa a new instance of this class
        /// </summary>
        /// <param name="validators">The list of validators to use</param>
        public OrValidator(params IValidatable[] validators) : base(validators) { }

        /// <inheritdoc/>
        public override bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return validatables.Any(validator => validator.IsValidEntity(entityId, entityManager));
        }
    }
}
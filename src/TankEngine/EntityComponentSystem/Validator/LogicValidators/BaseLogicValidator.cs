using System.Collections.Generic;
using System.Linq;
using TankEngine.EntityComponentSystem.Manager;

namespace TankEngine.EntityComponentSystem.Validator.LogicValidators
{
    /// <summary>
    /// Base class for logic validators
    /// </summary>
    public abstract class BaseLogicValidator : IValidatable
    {
        /// <summary>
        /// All the validatables to use for the validation
        /// </summary>
        protected readonly List<IValidatable> validatables;

        /// <summary>
        /// Create a new instance of this class
        /// <paramref name="validatables">A list with the validators to use</paramref>
        /// </summary>
        public BaseLogicValidator(List<IValidatable> validatables)
        {
            this.validatables = validatables ?? new List<IValidatable>();
        }

        /// <summary>
        /// Create a new instance of this class
        /// <paramref name="validatables">A list with the validators to use</paramref>
        /// </summary>
        public BaseLogicValidator(params IValidatable[] validators)
        {
            validatables = new List<IValidatable>();
            foreach (IValidatable validator in validators)
            {
                Add(validator);
            }
        }

        /// <summary>
        /// Add a new validateable
        /// </summary>
        /// <param name="validatableToAdd"></param>
        public void Add(IValidatable validatableToAdd)
        {
            if (validatables.Any(data => data.GetType() == validatableToAdd.GetType()))
            {
                return;
            }
            validatables.Add(validatableToAdd);
        }

        /// <inheritdoc/>
        public abstract bool IsValidEntity(uint entityId, IEntityManager entityManager);
    }
}

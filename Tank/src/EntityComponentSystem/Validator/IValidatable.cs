using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.src.EntityComponentSystem.Validator
{
    /// <summary>
    /// This interface describes a validator for systems
    /// </summary>
    interface IValidatable
    {
        /// <summary>
        /// Is an entity valid
        /// </summary>
        /// <param name="entityId">The id of the entity to check</param>
        /// <param name="entityManager">The manager instance to use for checking</param>
        /// <returns>True if an entity is valid</returns>
        bool IsValidEntity(uint entityId, IEntityManager entityManager);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Components;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.src.EntityComponentSystem.Validator;

namespace Tank.Validator
{
    /// <summary>
    /// Class to check if a object is controllable by the player
    /// </summary>
    class PlayerObjectValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(PlayerControllableComponent));

            return valid;
        }
    }
}

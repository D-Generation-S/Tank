using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Components;
using Tank.src.EntityComponentSystem.Validator;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.src.Validator
{
    /// <summary>
    /// This class will validate an entity as an sound effect
    /// </summary>
    class SoundEffectValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return entityManager.HasComponent(entityId, typeof(SoundEffectComponent));
        }
    }
}

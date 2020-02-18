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
    /// This class will validate entites which can be used by the animation system 
    /// </summary>
    class AnimationEntityRelevant : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(AnimationComponent));
            valid &= entityManager.HasComponent(entityId, typeof(VisibleComponent));
            return valid;
        }
    }
}

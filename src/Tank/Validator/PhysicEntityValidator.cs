using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Components;
using Tank.src.EntityComponentSystem.Validator;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    /// <summary>
    /// This class will validate all the entites which are changed by physic
    /// </summary>
    class PhysicEntityValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(PlaceableComponent));
            valid = entityManager.HasComponent(entityId, typeof(ColliderComponent));
            valid &= entityManager.HasComponent(entityId, typeof(MoveableComponent));
            return valid;
        }
    }
}

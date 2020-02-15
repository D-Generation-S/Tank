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
    class ColliderObjectValidator : IValidatable
    {
        public virtual bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(PlaceableComponent));
            valid &= entityManager.HasComponent(entityId, typeof(ColliderComponent));
            return valid;
        }
    }
}

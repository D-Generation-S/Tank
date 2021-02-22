using System;
using System.Collections.Generic;
using System.Text;
using Tank.Components;
using Tank.src.EntityComponentSystem.Validator;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    class BindEntityValidator : IValidatable
    {
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(PlaceableComponent));
            valid &= entityManager.HasComponent(entityId, typeof(BindComponent));
            return valid;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Tank.Components.Forces;
using Tank.EntityComponentSystem.Validator;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    class ForceValidator : IValidatable
    {
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return entityManager.HasComponent(entityId, typeof(ForceComponent));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Tank.Components;
using Tank.Components.GameObject;
using Tank.Components.Rendering;
using Tank.EntityComponentSystem.Validator;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    class AnimationAttributeDisplayValidator : IValidatable
    {
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent<BindComponent>(entityId);
            valid &= entityManager.HasComponent<AttributeDisplayComponent>(entityId);
            valid &= entityManager.HasComponent<VisibleComponent>(entityId);
            return valid;
        }
    }
}

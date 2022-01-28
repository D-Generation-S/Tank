﻿using Tank.Components;
using Tank.Components.GameObject;
using Tank.Components.Rendering;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace Tank.Validator
{
    class TextAttributeDisplayValidator : IValidatable
    {
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent<BindComponent>(entityId);
            valid &= entityManager.HasComponent<AttributeDisplayComponent>(entityId);
            valid &= entityManager.HasComponent<VisibleTextComponent>(entityId);
            return valid;
        }
    }
}
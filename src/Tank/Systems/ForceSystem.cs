using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.Components;
using Tank.Components.Forces;
using Tank.DataStructure;
using Tank.EntityComponentSystem.Validator;
using Tank.Enums;
using Tank.Events.PhysicBased;
using Tank.Validator;

namespace Tank.Systems
{
    internal class ForceSystem : AbstractSystem
    {
        private readonly IValidatable targetValidator;

        public ForceSystem()
        {
            validators.Add(new ForceValidator());
            targetValidator = new PhysicEntityValidator();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach(uint entityId in watchedEntities)
            {
                ForceComponent force = entityManager.GetComponent<ForceComponent>(entityId);
                PlaceableComponent origin = entityManager.GetComponent<PlaceableComponent>(entityId);
                if (force == null || origin == null)
                {
                    continue;
                }

                List<uint> targets = entityManager.GetEntitiesWithComponent<MoveableComponent>();
                targets.Remove(entityId);

                foreach (uint targetId in targets)
                {
                    if (!targetValidator.IsValidEntity(targetId, entityManager))
                    {
                        continue;
                    }

                    PlaceableComponent targetPosition = entityManager.GetComponent<PlaceableComponent>(targetId);
                    if (targetPosition == null)
                    {
                        continue;
                    }
                    Vector2 forceToApply = Vector2.Zero;
                    switch (force.ForceType)
                    {
                        case ForceTypeEnum.Push:
                            forceToApply = GetPushForce(force, origin, targetPosition);
                            break;
                        case ForceTypeEnum.Pull:
                            forceToApply = GetPullForce(force, origin, targetPosition);
                            break;
                        default:
                            break;
                    }
                    if (force.ForceTrigger == ForceTriggerTimeEnum.Add)
                    {
                        entityManager.RemoveComponents(entityId, force.GetType());
                    }
                    FireEvent(new ApplyForceEvent(targetId, forceToApply));
                }
            }
        }

        private Vector2 GetPushForce(ForceComponent force, PlaceableComponent origin, PlaceableComponent targetPosition)
        {
            return GetPullForce(force, origin, targetPosition) * -1;
        }

        private Vector2 GetPullForce(ForceComponent force, PlaceableComponent origin, PlaceableComponent targetPosition)
        {
            Circle circle = new Circle(origin.Position, (int)force.ForceRadius);
            if (!circle.IsInInCircle(targetPosition.Position))
            {
                return Vector2.Zero;
            }
            Vector2 direction = origin.Position - targetPosition.Position;
            float powerPercentage = direction.Length();
            powerPercentage /= force.ForceRadius;
            powerPercentage = 1 - powerPercentage;
            powerPercentage = Math.Clamp(powerPercentage, 0, 1);
            direction.Normalize();
            float strenght = force.ForceBaseStrengh * powerPercentage;
            return direction * strenght;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
        private readonly object componentLock;

        private Queue<AsyncComponentRemoveContainer> usedContainers;
        List<AsyncComponentRemoveContainer> componentsToRemove;

        public ForceSystem()
        {
            validators.Add(new ForceValidator());
            targetValidator = new PhysicEntityValidator();
            componentLock = new object();
            usedContainers = new Queue<AsyncComponentRemoveContainer>();
            componentsToRemove = new List<AsyncComponentRemoveContainer>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            List<uint> allTargets = entityManager.GetEntitiesWithComponent<MoveableComponent>();
            int processes = watchedEntities.Count;
            using (ManualResetEvent resetEvent = new ManualResetEvent(false))
            {
                if (processes == 0)
                {
                    resetEvent.Set();
                }
                foreach (uint entityId in watchedEntities)
                {
                    ThreadPool.QueueUserWorkItem(x =>
                    {
                        List<AsyncComponentRemoveContainer> dataToRemove = ApplyForces(entityId, allTargets);
                        lock (componentLock)
                        {
                            componentsToRemove.AddRange(dataToRemove);
                        }
                        if (Interlocked.Decrement(ref processes) == 0)
                            resetEvent.Set();
                    });
                }
                resetEvent.WaitOne();
            }
            foreach(AsyncComponentRemoveContainer container in componentsToRemove)
            {
                usedContainers.Enqueue(container);
                entityManager.RemoveComponents(container.EntityId, container.ComponentType);
            }
            componentsToRemove.Clear();
        }

        private List<AsyncComponentRemoveContainer> ApplyForces(uint entityId, List<uint> allTargets)
        {
            List<AsyncComponentRemoveContainer> componentsToRemove = new List<AsyncComponentRemoveContainer>();
            ForceComponent force = entityManager.GetComponent<ForceComponent>(entityId);
            PlaceableComponent origin = entityManager.GetComponent<PlaceableComponent>(entityId);
            if (force == null || origin == null)
            {
                return componentsToRemove;
            }

            foreach (uint targetId in allTargets)
            {
                if (!targetValidator.IsValidEntity(targetId, entityManager) || targetId == entityId)
                {
                    continue;
                }

                PlaceableComponent targetPosition = entityManager.GetComponent<PlaceableComponent>(targetId);
                if (targetPosition == null || !IsInCircle(origin.Position, force.ForceRadius, targetPosition.Position))
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
                    lock (componentLock)
                    {
                        AsyncComponentRemoveContainer container = GetContainer();
                        container.EntityId = entityId;
                        container.ComponentType = force.GetType();
                        componentsToRemove.Add(container);
                    }
                }
                FireEvent(new ApplyForceEvent(targetId, forceToApply));
            }
            return componentsToRemove;
        }

        private Vector2 GetPushForce(ForceComponent force, PlaceableComponent origin, PlaceableComponent targetPosition)
        {
            return GetPullForce(force, origin, targetPosition) * -1;
        }

        private Vector2 GetPullForce(ForceComponent force, PlaceableComponent origin, PlaceableComponent targetPosition)
        {
            Vector2 direction = origin.Position - targetPosition.Position;
            float powerPercentage = direction.Length();
            powerPercentage /= force.ForceRadius;
            powerPercentage = 1 - powerPercentage;
            powerPercentage = Math.Clamp(powerPercentage, 0, 1);
            direction.Normalize();
            float strenght = force.ForceBaseStrenght * powerPercentage;
            return direction * strenght;
        }

        private bool IsInCircle(Vector2 cirlceCenter, float circleRadius, Vector2 pointToCheck)
        {
            Vector2 distance = cirlceCenter - pointToCheck;
            return distance.Length() < circleRadius;
        }

        private AsyncComponentRemoveContainer GetContainer()
        {
            return usedContainers.Count > 0 ? usedContainers.Dequeue() : new AsyncComponentRemoveContainer();
        }
    }
}

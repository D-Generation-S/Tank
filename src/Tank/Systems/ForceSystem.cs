using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Force sytem class
    /// </summary>
    internal class ForceSystem : AbstractSystem
    {
        /// <summary>
        /// The validator to use for targets
        /// </summary>
        private readonly IValidatable targetValidator;

        /// <summary>
        /// Components current locked
        /// </summary>
        private readonly object componentLock;

        /// <summary>
        /// All the used container
        /// </summary>
        private Queue<AsyncComponentRemoveContainer> usedContainers;

        /// <summary>
        /// All the containers to remove after async part
        /// </summary>
        List<AsyncComponentRemoveContainer> componentsToRemove;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public ForceSystem()
        {
            validators.Add(new ForceValidator());
            targetValidator = new PhysicEntityValidator();
            componentLock = new object();
            usedContainers = new Queue<AsyncComponentRemoveContainer>();
            componentsToRemove = new List<AsyncComponentRemoveContainer>();
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Apply all the forces
        /// </summary>
        /// <param name="entityId">The entity id to apply the forces for</param>
        /// <param name="allTargets">The targets to check</param>
        /// <returns>A list with components to remove</returns>
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

        /// <summary>
        /// Get the push force
        /// </summary>
        /// <param name="force">The force component</param>
        /// <param name="origin">The position of the object</param>
        /// <param name="targetPosition">The position of the target</param>
        /// <returns>The force to apply on the target</returns>
        private Vector2 GetPushForce(ForceComponent force, PlaceableComponent origin, PlaceableComponent targetPosition)
        {
            return GetPullForce(force, origin, targetPosition) * -1;
        }

        /// <summary>
        /// Get pull forces
        /// </summary>
        /// <param name="force">The force component</param>
        /// <param name="origin">The position of the object</param>
        /// <param name="targetPosition">The position of the target</param>
        /// <returns>The force to apply on the target</returns>
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

        /// <summary>
        /// Is the point in the circle
        /// </summary>
        /// <param name="cirlceCenter">Center of the circle</param>
        /// <param name="circleRadius">Circle radius</param>
        /// <param name="pointToCheck">The point to check</param>
        /// <returns></returns>
        private bool IsInCircle(Vector2 cirlceCenter, float circleRadius, Vector2 pointToCheck)
        {
            Vector2 distance = cirlceCenter - pointToCheck;
            return distance.Length() < circleRadius;
        }

        /// <summary>
        /// Get a new container or a used one
        /// </summary>
        /// <returns>A new or used container</returns>
        private AsyncComponentRemoveContainer GetContainer()
        {
            return usedContainers.Count > 0 ? usedContainers.Dequeue() : new AsyncComponentRemoveContainer();
        }
    }
}

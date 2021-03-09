using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Forces;
using Tank.DataStructure.Geometrics;
using Tank.DataStructure.Quadtree;
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
        /// The quadtree to use
        /// </summary>
        private readonly QuadTree quadTree;

        /// <summary>
        /// The used data containers
        /// </summary>
        private Queue<QuadTreeData> usedDataContainer;

        /// <summary>
        /// The currently active data containers
        /// </summary>
        private List<QuadTreeData> activeDataContainer;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public ForceSystem(VectorRectangle forceBounding)
            :this(new QuadTree(forceBounding, 4))
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public ForceSystem(QuadTree quadTree)
        {
            validators.Add(new ForceValidator());
            targetValidator = new PhysicEntityValidator();
            usedDataContainer = new Queue<QuadTreeData>();
            activeDataContainer = new List<QuadTreeData>();
            for (int i = 0; i < 20; i++)
            {
                usedDataContainer.Enqueue(new QuadTreeData());
            }

            this.quadTree = quadTree;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (watchedEntities.Count == 0)
            {
                return;
            }
            quadTree.Clear();
            List<uint> allTargets = entityManager.GetEntitiesWithComponent<MoveableComponent>();
            for (int i = 0; i < allTargets.Count; i++)
            {
                uint entityId = allTargets[i];
                PlaceableComponent placeable = entityManager.GetComponent<PlaceableComponent>(entityId);
                QuadTreeData quadTreeData = GetDataContainer(placeable.Position, entityId);
                quadTree.Insert(quadTreeData);
                activeDataContainer.Add(quadTreeData);
            }

            foreach (uint entityId in watchedEntities)
            {
                ForceComponent force = entityManager.GetComponent<ForceComponent>(entityId);
                PlaceableComponent placeable = entityManager.GetComponent<PlaceableComponent>(entityId);
                ApplyForces(entityId, force, placeable);
            }

            foreach(QuadTreeData treeData in activeDataContainer)
            {
                usedDataContainer.Enqueue(treeData);
            }
            activeDataContainer.Clear();
        }

        /// <summary>
        /// Apply the forces for every entity
        /// </summary>
        /// <param name="entityId">The entity id to get the forces from</param>
        /// <param name="force">The force component</param>
        /// <param name="origin">The origin of the entity to apply forces from</param>
        private void ApplyForces(uint entityId, ForceComponent force, PlaceableComponent origin)
        {
            if (force == null || origin == null)
            {
                return;
            }
            VectorRectangle searchArea = new VectorRectangle(origin.Position, force.ForceDiameter * 2);
            int count = 0;
            foreach (QuadTreeData data in quadTree.Query(searchArea))
            {
                uint targetId = data.GetData<uint>();
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
                    entityManager.RemoveComponents(entityId, force.Type);
                }

                FireEvent(new ApplyForceEvent(targetId, forceToApply));
            }
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
        /// Get a new quadtree data container
        /// </summary>
        /// <returns>A quadtree data container</returns>
        private QuadTreeData GetDataContainer(Vector2 position, object data)
        {
            QuadTreeData returnData = usedDataContainer.Count > 0 ? usedDataContainer.Dequeue() : new QuadTreeData();
            returnData.Init(position, data);
            return returnData;
        }
    }
}

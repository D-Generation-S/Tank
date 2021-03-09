using System;
using System.Collections.Generic;
using Tank.Events.ComponentBased;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.EntityComponentSystem.DataContainer
{
    /// <summary>
    /// Class to store components of a given type
    /// </summary>
    class ComponentStorage : IClearable
    {
        /// <summary>
        /// The type of components to store
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// The stored components
        /// </summary>
        private readonly List<IComponent> components;

        /// <summary>
        /// The number of components
        /// </summary>
        public int ComponentCount => components.Count;

        /// <summary>
        /// The event manager to use
        /// </summary>
        private readonly IEventManager eventManager;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventManager"></param>
        public ComponentStorage(Type type, IEventManager eventManager)
        {
            Type = type;
            this.eventManager = eventManager;
            components = new List<IComponent>();
        }

        /// <summary>
        /// Add a new component
        /// </summary>
        /// <param name="entityId">The id to add the component to</param>
        /// <param name="component">The component to add</param>
        /// <returns>True if adding was a success</returns>
        public bool AddComponent(uint entityId, IComponent component)
        {
            if (!component.AllowMultiple && HasComponent(entityId))
            {
                return false;
            }
            component.SetEntityId(entityId);
            components.Add(component);
            return true;
        }

        /// <summary>
        /// Has a given entity a specific component
        /// </summary>
        /// <param name="entityId">The entity to check</param>
        /// <returns>True if the entity do have the component</returns>
        public bool HasComponent(uint entityId)
        {
            return GetComponent(entityId) != null;
        }

        /// <summary>
        /// Get a single component for the given entity
        /// </summary>
        /// <param name="entityId">The entity to search in</param>
        /// <returns>True if component was found</returns>
        public IComponent GetComponent(uint entityId)
        {
            return components.Find(component =>
            {
                return component != null && component.EntityId == entityId;
            });
        }

        /// <summary>
        /// Get all the component of the entity id
        /// </summary>
        /// <param name="entityId">The entity id to get all components</param>
        /// <returns>All the components of the entity</returns>
        public List<IComponent> GetComponents(uint entityId)
        {
            return components.FindAll((componentToCheck) =>
            {
                return componentToCheck != null && componentToCheck.EntityId == entityId;
            });
        }

        /// <summary>
        /// Move a components to a new entity
        /// </summary>
        /// <param name="targetEntityId">The target entity for the new component</param>
        /// <param name="componentToMove">The component to move</param>
        /// <returns></returns>
        public bool MoveComponents(uint targetEntityId, IComponent componentToMove)
        {
            // Not sure if this method is really required here!
            eventManager.FireEvent(this, new ComponentRemovedEvent(componentToMove.EntityId));
            componentToMove.SetEntityId(targetEntityId);
            eventManager.FireEvent(this, new NewComponentEvent(targetEntityId));

            return true;
        }

        /// <summary>
        /// Get all entites with the component
        /// </summary>
        /// <returns>A list with all entites</returns>
        public List<uint> GetEntitesWithComponent()
        {
            List<uint> returnEntites = new List<uint>();
            foreach(IComponent component in components)
            {
                if (returnEntites.Contains(component.EntityId))
                {
                    continue;
                }
                returnEntites.Add(component.EntityId);
            }
            return returnEntites;
        }

        /// <summary>
        /// Remove a specific entity
        /// </summary>
        /// <param name="component">The component to remove</param>
        public void Remove(IComponent component)
        {
            components.Remove(component);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            components.Clear();
        }
    }
}

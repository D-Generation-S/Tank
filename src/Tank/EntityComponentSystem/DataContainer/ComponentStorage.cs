using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Dictionary with all the components of a single type
        /// </summary>
        private readonly Dictionary<uint, List<IComponent>> componentDictonary;

        /// <summary>
        /// The number of components
        /// </summary>
        public int ComponentCount => componentDictonary.Count;

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
            componentDictonary = new Dictionary<uint, List<IComponent>>();
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
            if (componentDictonary.ContainsKey(entityId))
            {
                componentDictonary[entityId].Add(component);
                return true;
            }
            componentDictonary.Add(entityId, new List<IComponent>() { component });
            return true;
        }

        /// <summary>
        /// Remove a specific entity
        /// </summary>
        /// <param name="component">The component to remove</param>
        public void Remove(uint entityId, IComponent component)
        {
            if (!componentDictonary.ContainsKey(entityId))
            {
                return;
            }
            componentDictonary[entityId].Remove(component);
            if (componentDictonary[entityId].Count == 0)
            {
                componentDictonary.Remove(entityId);
            }
        }

        /// <summary>
        /// Has a given entity a specific component
        /// </summary>
        /// <param name="entityId">The entity to check</param>
        /// <returns>True if the entity do have the component</returns>
        public bool HasComponent(uint entityId)
        {
            return componentDictonary.ContainsKey(entityId) && componentDictonary[entityId].Count > 0;
        }

        /// <summary>
        /// Get a single component for the given entity
        /// </summary>
        /// <param name="entityId">The entity to search in</param>
        /// <returns>True if component was found</returns>
        public IComponent GetComponent(uint entityId)
        {
            if (!componentDictonary.ContainsKey(entityId))
            {
                return default;
            }
            List<IComponent> components = componentDictonary[entityId];
            return components.FirstOrDefault();
        }

        /// <summary>
        /// Get all the component of the entity id
        /// </summary>
        /// <param name="entityId">The entity id to get all components</param>
        /// <returns>All the components of the entity</returns>
        public List<IComponent> GetComponents(uint entityId)
        {
            if (!componentDictonary.ContainsKey(entityId))
            {
                return new List<IComponent>();
            }
            return componentDictonary[entityId];
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
            ComponentChangedEvent componentRemovedEvent = eventManager.CreateEvent<ComponentChangedEvent>();
            componentRemovedEvent.EntityId = componentToMove.EntityId;
            eventManager.FireEvent(this, componentRemovedEvent);
            componentToMove.SetEntityId(targetEntityId);
            ComponentChangedEvent newComponentEvent = eventManager.CreateEvent<ComponentChangedEvent>();
            newComponentEvent.EntityId = componentToMove.EntityId;
            eventManager.FireEvent(this, newComponentEvent);
            return true;
        }

        /// <summary>
        /// Get all entites with the component
        /// </summary>
        /// <returns>A list with all entites</returns>
        public List<uint> GetEntitesWithComponent()
        {
            return componentDictonary.Keys.ToList();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            componentDictonary.Clear();
        }
    }
}

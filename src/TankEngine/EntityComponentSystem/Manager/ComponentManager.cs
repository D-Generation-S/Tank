using System;
using System.Collections.Generic;
using TankEngine.EntityComponentSystem.DataContainer;
using TankEngine.EntityComponentSystem.Events;

namespace TankEngine.EntityComponentSystem.Manager
{
    /// <summary>
    /// The component manager 
    /// </summary>
    public class ComponentManager : IComponentManager
    {
        /// <summary>
        /// The component storage to use
        /// </summary>
        List<ComponentStorage> componentStorages;

        /// <summary>
        /// The event manager to use
        /// </summary>
        private IEventManager eventManager;

        /// <summary>
        /// A list with all the used components
        /// </summary>
        private readonly List<IComponent> usedComponents;

        /// <summary>
        /// The maximum amount of components to keep
        /// </summary>
        private readonly int maxComponentsToKeep;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public ComponentManager()
        {
            componentStorages = new List<ComponentStorage>();
            usedComponents = new List<IComponent>();
            maxComponentsToKeep = 250000;
        }

        /// <inheritdoc/>
        public void Initialize(IEventManager eventManager)
        {
            this.eventManager = eventManager;
        }

        /// <summary>
        /// Get the component storage for the given type
        /// </summary>
        /// <param name="type">The type of the storage to get</param>
        /// <returns>The correct storage</returns>
        private ComponentStorage GetComponentStorage(Type type)
        {
            return componentStorages.Find(storage => storage.Type == type);
        }

        /// <inheritdoc/>
        public bool AddComponent(uint entityId, IComponent component)
        {
            ComponentStorage storage = GetComponentStorage(component.Type);
            if (storage == null)
            {
                storage = new ComponentStorage(component.Type, eventManager);
                componentStorages.Add(storage);
            }
            return storage.AddComponent(entityId, component);
        }

        /// <inheritdoc/>
        public IComponent GetComponent(uint entityId, Type componentType)
        {
            ComponentStorage storage = GetComponentStorage(componentType);
            if (storage == null)
            {
                return null;
            }
            return storage.GetComponent(entityId);
        }

        /// <inheritdoc/>
        public T GetComponent<T>(uint entityId) where T : IComponent
        {
            IComponent component = GetComponent(entityId, typeof(T));
            if (component == null)
            {
                return default;
            }
            return (T)component;

        }

        /// <inheritdoc/>
        public List<IComponent> GetComponents(uint entityId)
        {
            List<IComponent> components = new List<IComponent>();
            foreach (ComponentStorage storage in componentStorages)
            {
                components.AddRange(storage.GetComponents(entityId));
            }
            return components;
        }

        /// <inheritdoc/>
        public List<IComponent> GetComponents(uint entityId, Type componentType)
        {
            ComponentStorage storage = GetComponentStorage(componentType);
            if (storage == null)
            {
                return new List<IComponent>();
            }
            return storage.GetComponents(entityId);
        }

        /// <inheritdoc/>
        public List<IComponent> GetComponents(Type componentType)
        {
            ComponentStorage storage = GetComponentStorage(componentType);
            if (storage == null)
            {
                return new List<IComponent>();
            }
            return storage.GetComponents();
        }

        /// <inheritdoc/>
        public List<uint> GetEntitiesWithComponent(Type componentType)
        {
            ComponentStorage storage = GetComponentStorage(componentType);
            if (storage == null)
            {
                return new List<uint>();
            }
            return storage.GetEntitesWithComponent();
        }

        /// <inheritdoc/>
        public bool HasComponent(uint entityId, Type componentType)
        {
            ComponentStorage storage = GetComponentStorage(componentType);
            if (storage == null)
            {
                return false;
            }
            return storage.HasComponent(entityId);
        }

        /// <inheritdoc/>
        public bool MoveComponent(uint targetEntityId, IComponent componentToMove)
        {
            componentToMove.SetEntityId(targetEntityId);
            return true;
        }

        /// <inheritdoc/>
        public bool MoveComponent(uint sourceEntity, uint targetEntityId, Type type)
        {
            List<IComponent> components = GetComponentStorage(type).GetComponents(sourceEntity);
            bool returnValue = true;
            foreach (IComponent componentToMove in components)
            {
                returnValue &= MoveComponent(targetEntityId, componentToMove);
            }
            return returnValue;
        }

        /// <inheritdoc/>
        public void RemoveComponents(uint entityId, Type componentType)
        {
            RemoveComponents(entityId, componentType, true);
        }


        /// <summary>
        /// Remove a component type from an entity
        /// </summary>
        /// <param name="entityId">The entity id to remove the components from</param>
        /// <param name="componentType">The type of component to remove</param>
        /// <param name="informSystem">Should we send an event to inform every subscriber</param>
        private void RemoveComponents(uint entityId, Type componentType, bool informSystem)
        {
            ComponentStorage storage = GetComponentStorage(componentType);
            List<IComponent> componentsToRemove = new List<IComponent>();
            foreach (IComponent removeComponent in storage.GetComponents(entityId))
            {
                AddRemovedComponent(removeComponent);
                componentsToRemove.Add(removeComponent);
            }
            componentsToRemove.ForEach(data => storage.Remove(entityId, data));
            if (informSystem)
            {
                ComponentChangedEvent componentRemovedEvent = eventManager.CreateEvent<ComponentChangedEvent>();
                componentRemovedEvent.EntityId = entityId;
                eventManager.FireEvent(this, componentRemovedEvent);
            }
        }

        /// <inheritdoc/>
        public void RemoveComponents(uint entityId)
        {
            foreach (ComponentStorage storage in componentStorages)
            {
                RemoveComponents(entityId, storage.Type, false);
            }
            ComponentChangedEvent componentRemovedEvent = eventManager.CreateEvent<ComponentChangedEvent>();
            componentRemovedEvent.EntityId = entityId;
            eventManager.FireEvent(this, componentRemovedEvent);
        }

        /// <inheritdoc/>
        public int GetComponentCount()
        {
            int count = 0;
            foreach (ComponentStorage storage in componentStorages)
            {
                count += storage.ComponentCount;
            }
            return count;
        }

        /// <inheritdoc/>
        public int GetUsedComponentCount()
        {
            return usedComponents.Count;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            componentStorages.ForEach(storage => storage.Clear());
        }

        /// <summary>
        /// Add a new component to the used list
        /// </summary>
        /// <param name="component">The component to add</param>
        private void AddRemovedComponent(IComponent component)
        {
            if (usedComponents.Count >= maxComponentsToKeep)
            {
                usedComponents.RemoveAt(0);
            }
            usedComponents.Add(component);
        }

        /// <inheritdoc/>
        public T CreateComponent<T>(uint entityId) where T : IComponent
        {
            T component = CreateComponent<T>();
            AddComponent(entityId, component);
            return component;
        }

        /// <inheritdoc/>
        public T CreateComponent<T>() where T : IComponent
        {
            Type type = typeof(T);
            T component = (T)usedComponents.Find(item => item.Type == type);
            if (component != null)
            {
                usedComponents.Remove(component);
                component.Init();
            }
            return component == null ? (T)Activator.CreateInstance(type) : component;
        }

    }
}

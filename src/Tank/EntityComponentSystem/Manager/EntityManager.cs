using System;
using System.Collections.Generic;
using System.Linq;
using Tank.Events.ComponentBased;
using Tank.Events.EntityBased;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.EntityComponentSystem.Manager
{
    /// <summary>
    /// This class is the default entity manager used inside of the game engine
    /// </summary>
    class EntityManager : IEntityManager, IEventReceiver
    {
        /// <summary>
        /// All list with all the active entities
        /// </summary>
        private readonly List<uint> entities;

        /// <summary>
        /// All the components which are currently available
        /// </summary>
        private readonly List<IComponent> components;

        /// <summary>
        /// A list with all the used components
        /// </summary>
        private readonly List<IComponent> usedComponents;

        /// <summary>
        /// The maximum amount of components to keep
        /// </summary>
        private readonly int maxComponentsToKeep;

        /// <summary>
        /// All the ids which already got used
        /// </summary>
        private readonly Queue<uint> removedEntities;

        /// <summary>
        /// The next entity id to create
        /// </summary>
        private uint nextId;

        /// <summary>
        /// An instance of the event manager to register to events
        /// </summary>
        private IEventManager eventManager;

        /// <summary>
        /// The current component actions
        /// </summary>
        private uint componentActions;

        /// <summary>
        /// How many actions until we need to sort
        /// </summary>
        private readonly uint sortThreshold;

        /// <summary>
        /// Create a new instance of the entity manager
        /// </summary>
        public EntityManager()
        {
            entities = new List<uint>();
            components = new List<IComponent>();
            removedEntities = new Queue<uint>();
            usedComponents = new List<IComponent>();
            maxComponentsToKeep = 200;
            sortThreshold = 200;
            nextId = uint.MinValue;
        }

        /// <summary>
        /// Initialize the entity manager
        /// </summary>
        /// <param name="eventManager">The event manager to use</param>
        public void Initialize(IEventManager eventManager)
        {
            this.eventManager = eventManager;
            eventManager.SubscribeEvent(this, typeof(RemoveEntityEvent));
            eventManager.SubscribeEvent(this, typeof(RemoveComponentEvent));
            eventManager.SubscribeEvent(this, typeof(AddEntityEvent));
        }

        /// <inheritdoc/>
        public uint CreateEntity()
        {
            return CreateEntity(true);
        }

        /// <inheritdoc/>
        public uint CreateEntity(bool notifySystems)
        {
            uint idToReturn = nextId;
            if (removedEntities.Count > 0)
            {
                idToReturn = removedEntities.Dequeue();
                entities.Add(idToReturn);
                if (notifySystems)
                {
                    eventManager.FireEvent(this, new NewEntityEvent(idToReturn));
                }

                return idToReturn;
            }

            nextId++;
            entities.Add(idToReturn); if (notifySystems)
            {
                eventManager.FireEvent(this, new NewEntityEvent(idToReturn));
            }
            return idToReturn;
        }

        /// <inheritdoc/>
        public bool EntityExists(uint entityId)
        {
            return entities.Contains(entityId);
        }

        /// <inheritdoc/>
        public List<uint> GetEntitiesWithComponent<T>() where T : IComponent
        {
            Type type = typeof(T);
            return entities.FindAll((entity) =>
            {
                return HasComponent(entity, type);
            });
        }

        /// <inheritdoc/>
        public bool AddComponent(uint entityId, IComponent component)
        {
            return AddComponent(entityId, component, true);
        }

        /// <inheritdoc/>
        public bool AddComponent(uint entityId, IComponent component, bool informSystems)
        {
            if (!component.AllowMultiple && HasComponent(entityId, component))
            {
                return false;
            }

            component.SetEntityId(entityId);
            components.Add(component);
            if (informSystems)
            {
                eventManager.FireEvent(this, new NewComponentEvent(entityId));
            }
            componentActions++;
            if (componentActions > sortThreshold)
            {
                componentActions = 0;
                OptimizeComponents();
            }
            return true;
        }

        /// <inheritdoc/>
        public T GetComponent<T>(uint entityId) where T : IComponent
        {
            IComponent returnComponent = GetComponent(entityId, typeof(T));
            if (returnComponent == null)
            {
                return default;
            }
            return (T)returnComponent;
        }

        /// <inheritdoc/>
        public IComponent GetComponent(uint entityId, Type componentType)
        {
            return components.Find(component =>
            {
                return component != null && component.Type == componentType && component.EntityId == entityId;
            });
        }

        /// <inheritdoc/>
        public IComponent GetComponent(uint entityId, IComponent component)
        {
            return GetComponent(entityId, component.Type);
        }

        /// <inheritdoc/>
        public List<IComponent> GetComponents(uint entityId)
        {
            return components.FindAll((component) =>
            {
                return component != null && component.EntityId == entityId;
            });
        }

        /// <inheritdoc/>
        public List<IComponent> GetComponents(uint entityId, Type componentType)
        {
            return components.FindAll((componentToCheck) =>
            {
                if (componentToCheck == null)
                {
                    return false;
                }
                bool matchingComponent = componentToCheck.Type == componentType;
                matchingComponent &= componentToCheck.EntityId == entityId;
                return matchingComponent;
            });
        }

        /// <inheritdoc/>
        public List<IComponent> GetComponents(uint entityId, IComponent component)
        {
            return GetComponents(entityId, component.Type);
        }

        /// <inheritdoc/>
        public bool HasComponent(uint entityId, Type component)
        {
            return GetComponent(entityId, component) != null;
        }

        /// <inheritdoc/>
        public bool HasComponent(uint entityId, IComponent component)
        {
            return HasComponent(entityId, component.Type);
        }

        /// <inheritdoc/>
        public bool MoveComponent<T>(uint sourceEntity, uint targetEntityId) where T : IComponent
        {
            if (!entities.Contains(sourceEntity))
            {
                return false;
            }
            Type type = typeof(T);
            List<IComponent> components = GetComponents(sourceEntity, type);
            bool returnValue = true;
            foreach (IComponent componentToMove in components)
            {
                returnValue &= MoveComponent(targetEntityId, componentToMove);
            }

            return returnValue;
        }

        /// <inheritdoc/>
        public bool MoveComponent(uint targetEntityId, IComponent componentToMove)
        {
            if (!entities.Contains(targetEntityId))
            {
                return false;
            }

            eventManager.FireEvent(this, new ComponentRemovedEvent(componentToMove.EntityId));
            componentToMove.SetEntityId(targetEntityId);
            eventManager.FireEvent(this, new NewComponentEvent(targetEntityId));

            return true;
        }

        /// <inheritdoc/>
        public void RemoveComponents(uint entityId, Type componentType)
        {
            foreach (IComponent removeComponent in GetComponents(entityId, componentType))
            {
                AddRemovedComponent(removeComponent);
                components.Remove(removeComponent);
            }
            eventManager.FireEvent(this, new ComponentRemovedEvent(entityId));
        }

        /// <inheritdoc/>
        public void RemoveComponents(uint entityId, IComponent component)
        {
            RemoveComponents(entityId, component.Type);
        }

        /// <inheritdoc/>
        public void RemoveComponents(uint entityId)
        {
            foreach (IComponent removeComponent in GetComponents(entityId))
            {
                AddRemovedComponent(removeComponent);
                components.Remove(removeComponent);
            }
            eventManager.FireEvent(this, new ComponentRemovedEvent(entityId));
        }

        /// <inheritdoc/>
        private void AddRemovedComponent(IComponent component)
        {
            if (usedComponents.Count >= maxComponentsToKeep)
            {
                usedComponents.RemoveAt(0);
            }
            usedComponents.Add(component);
        }

        /// <inheritdoc/>
        public void RemoveEntity(uint entityId)
        {
            RemoveComponents(entityId);
            entities.Remove(entityId);
            eventManager.FireEvent(this, new EntityRemovedEvent(entityId));
            removedEntities.Enqueue(entityId);
        }

        /// <inheritdoc/>
        public void EventNotification(object sender, EventArgs eventArgs)
        {
            if (eventArgs is AddEntityEvent)
            {
                uint entityId = CreateEntity(false);
                AddEntityEvent addEntityEvent = (AddEntityEvent)eventArgs;
                foreach (IComponent gameComponent in addEntityEvent.Components)
                {
                    AddComponent(entityId, gameComponent, false);
                }

                eventManager.FireEvent(this, new NewComponentEvent(entityId));

                return;
            }

            if (eventArgs is RemoveComponentEvent)
            {
                RemoveComponentEvent componentRemoveEvent = (RemoveComponentEvent)eventArgs;
                RemoveComponents(componentRemoveEvent.EntityId, componentRemoveEvent.ComponentType);
                return;
            }

            if (eventArgs is RemoveEntityEvent)
            {
                RemoveEntityEvent removeEntityEvent = (RemoveEntityEvent)eventArgs;
                RemoveEntity(removeEntityEvent.EntityId);
            }
        }

        /// <inheritdoc/>
        public int GetEntityCount()
        {
            return entities.Count;
        }

        /// <inheritdoc/>
        public int GetComponentCount()
        {
            return components.Count;
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

        /// <inheritdoc/>
        public T CreateComponent<T>(uint entityId) where T : IComponent
        {
            T component = CreateComponent<T>();
            AddComponent(entityId, component);
            return component;
        }

        /// <inheritdoc/>
        public int GetUsedComponentCount()
        {
            return usedComponents.Count;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                RemoveEntity(entities[i]);
            }
            usedComponents.Clear();
            components.Clear();
            removedEntities.Clear();
            nextId = 0;
        }

        /// <summary>
        /// Optimize the components
        /// </summary>
        public void OptimizeComponents()
        {
            components.Sort((componentA, componentB) => componentB.Priority - componentA.Priority);
        }
    }
}

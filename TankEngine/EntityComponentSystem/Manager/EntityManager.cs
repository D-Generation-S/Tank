using System;
using System.Collections.Generic;
using System.Linq;
using Tank.Events;
using Tank.Events.ComponentBased;
using Tank.Events.EntityBased;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.EntityComponentSystem.Manager
{
    /// <summary>
    /// This class is the default entity manager used inside of the game engine
    /// </summary>
    public class EntityManager : IEntityManager, IEventReceiver
    {
        /// <summary>
        /// All list with all the active entities
        /// </summary>
        private readonly List<uint> entities;

        /// <summary>
        /// The component manager to use
        /// </summary>
        private readonly IComponentManager componentManager;

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
        /// The stack with all entities to remove
        /// </summary>
        private Stack<uint> removeStack;

        /// <summary>
        /// Create a new instance of the entity manager
        /// </summary>
        public EntityManager()
        {
            entities = new List<uint>();

            componentManager = new ComponentManager();
            removedEntities = new Queue<uint>();
            nextId = uint.MinValue;
            removeStack = new Stack<uint>();
        }

        /// <summary>
        /// Initialize the entity manager
        /// </summary>
        /// <param name="eventManager">The event manager to use</param>
        public void Initialize(IEventManager eventManager)
        {
            this.eventManager = eventManager;
            componentManager.Initialize(eventManager);
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
                    NewEntityEvent newEntityEvent = eventManager.CreateEvent<NewEntityEvent>();
                    newEntityEvent.EntityId = idToReturn;
                    eventManager.FireEvent(this, newEntityEvent);
                }

                return idToReturn;
            }

            nextId++;
            entities.Add(idToReturn); if (notifySystems)
            {
                NewEntityEvent newEntityEvent = eventManager.CreateEvent<NewEntityEvent>();
                newEntityEvent.EntityId = idToReturn;
                eventManager.FireEvent(this, newEntityEvent);
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
            return componentManager.GetEntitiesWithComponent(typeof(T));
        }

        /// <inheritdoc/>
        public bool AddComponent(uint entityId, IComponent component)
        {
            return AddComponent(entityId, component, true);
        }

        /// <inheritdoc/>
        public bool AddComponent(uint entityId, IComponent component, bool informSystems)
        {
            bool returnValue = componentManager.AddComponent(entityId, component);
            if (!returnValue)
            {
                return returnValue;
            }
            if (informSystems)
            {
                ComponentChangedEvent newComponentEvent = eventManager.CreateEvent<ComponentChangedEvent>();
                newComponentEvent.EntityId = entityId;
                eventManager.FireEvent(this, newComponentEvent);
            }
            return returnValue;
        }

        /// <inheritdoc/>
        public T GetComponent<T>(uint entityId) where T : IComponent
        {
            return componentManager.GetComponent<T>(entityId);
        }

        /// <inheritdoc/>
        public IComponent GetComponent(uint entityId, Type componentType)
        {
            return componentManager.GetComponent(entityId, componentType);
        }

        /// <inheritdoc/>
        public IComponent GetComponent(uint entityId, IComponent component)
        {
            return GetComponent(entityId, component.Type);
        }

        /// <inheritdoc/>
        public List<IComponent> GetComponents(uint entityId)
        {
            return componentManager.GetComponents(entityId);
        }

        /// <inheritdoc/>
        public List<IComponent> GetComponents(uint entityId, Type componentType)
        {
            return componentManager.GetComponents(entityId, componentType);
        }

        /// <inheritdoc/>
        public List<IComponent> GetComponents(uint entityId, IComponent component)
        {
            return GetComponents(entityId, component.Type);
        }

        /// <inheritdoc/>
        public List<T> GetComponents<T>() where T : IComponent
        {
            return componentManager.GetComponents(typeof(T)).Cast<T>().ToList();
        }

        /// <inheritdoc/>
        public bool HasComponent<T>(uint entityId) where T : IComponent
        {
            return HasComponent(entityId, typeof(T));
        }

        /// <inheritdoc/>
        public bool HasComponent(uint entityId, IComponent component)
        {
            return HasComponent(entityId, component.Type);
        }

        /// <inheritdoc/>
        public bool HasComponent(uint entityId, Type component)
        {
            return componentManager.HasComponent(entityId, component);
        }

        /// <inheritdoc/>
        public bool MoveComponent<T>(uint sourceEntity, uint targetEntityId) where T : IComponent
        {
            if (!entities.Contains(sourceEntity))
            {
                return false;
            }

            return componentManager.MoveComponent(sourceEntity, targetEntityId, typeof(T));
        }

        /// <inheritdoc/>
        public bool MoveComponent(uint targetEntityId, IComponent componentToMove)
        {
            if (!entities.Contains(targetEntityId))
            {
                return false;
            }

            return componentManager.MoveComponent(targetEntityId, componentToMove);
        }

        /// <inheritdoc/>
        public void RemoveComponents(uint entityId, Type componentType)
        {
            componentManager.RemoveComponents(entityId, componentType);
        }

        /// <inheritdoc/>
        public void RemoveComponents(uint entityId, IComponent component)
        {
            RemoveComponents(entityId, component.Type);
        }

        /// <inheritdoc/>
        public void RemoveComponents(uint entityId)
        {
            componentManager.RemoveComponents(entityId);
        }

        /// <inheritdoc/>
        public void RemoveEntity(uint entityId)
        {
            removeStack.Push(entityId);
            return;
        }

        /// <inheritdoc/>
        public void EventNotification(object sender, IGameEvent eventArgs)
        {
            if (eventArgs is AddEntityEvent addEntityEvent)
            {
                uint entityId = CreateEntity(false);
                foreach (IComponent gameComponent in addEntityEvent.Components)
                {
                    AddComponent(entityId, gameComponent, false);
                }
                ComponentChangedEvent newComponentEvent = eventManager.CreateEvent<ComponentChangedEvent>();
                newComponentEvent.EntityId = entityId;
                eventManager.FireEvent(this, newComponentEvent);

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
            return componentManager.GetComponentCount();
        }

        /// <inheritdoc/>
        public T CreateComponent<T>() where T : IComponent
        {
            return componentManager.CreateComponent<T>();
        }

        /// <inheritdoc/>
        public T CreateComponent<T>(uint entityId) where T : IComponent
        {
            return componentManager.CreateComponent<T>(entityId);
        }

        /// <inheritdoc/>
        public int GetUsedComponentCount()
        {
            return componentManager.GetUsedComponentCount();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                RemoveEntity(entities[i]);
            }
            removedEntities.Clear();
            componentManager.Clear();
            nextId = 0;
        }

        public void LateUpdate()
        {
            while (removeStack.Count > 0)
            {
                uint entityId = removeStack.Pop();
                RemoveComponents(entityId);
                entities.Remove(entityId);
                EntityRemovedEvent entityRemovedEvent = eventManager.CreateEvent<EntityRemovedEvent>();
                entityRemovedEvent.EntityId = entityId;
                eventManager.FireEvent(this, entityRemovedEvent);
                removedEntities.Enqueue(entityId);
            }


        }
    }
}

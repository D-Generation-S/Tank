using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Events.ComponentBased;
using Tank.src.Events.EntityBased;
using Tank.src.Interfaces.EntityComponentSystem;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.src.EntityComponentSystem.Manager
{
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
        /// All the ids which already got used
        /// </summary>
        private readonly Queue<uint> removedEntities;

        private uint nextId;

        private IEventManager eventManager;

        public EntityManager()
        {
            entities = new List<uint>();
            components = new List<IComponent>();
            removedEntities = new Queue<uint>();
            
            nextId = uint.MinValue;
        }


        public void Initialize(IEventManager eventManager)
        {
            this.eventManager = eventManager;
            eventManager.SubscribeEvent(this, typeof(RemoveEntityEvent));
            eventManager.SubscribeEvent(this, typeof(RemoveComponentEvent));
            eventManager.SubscribeEvent(this, typeof(AddEntityEvent));
        }

        public uint CreateEntity()
        {
            uint idToReturn = nextId;
            if (removedEntities.Count > 0)
            {
                idToReturn = removedEntities.Dequeue();
                entities.Add(idToReturn);
                eventManager.FireEvent<NewEntityEvent>(this, new NewEntityEvent(idToReturn));
                return idToReturn;
            }

            nextId++;
            entities.Add(idToReturn);
            eventManager.FireEvent<NewEntityEvent>(this, new NewEntityEvent(idToReturn));
            return idToReturn;
        }



        public bool EntityExists(uint entityId)
        {
            return entities.Contains(entityId);
        }

        public bool AddComponent(uint entityId, IComponent component)
        {
            return AddComponent(entityId, component, true);
        }

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
                eventManager.FireEvent<NewComponentEvent>(this, new NewComponentEvent(entityId));
            }
            
            return true;
        }

        public T GetComponent<T>(uint entityId) where T : IComponent
        {
            IComponent returnComponent = GetComponents(entityId, typeof(T)).FirstOrDefault();
            if (returnComponent == null)
            {
                return default(T);
            }
            return (T)returnComponent;
        }

        public IComponent GetComponent(uint entityId, Type componentType)
        {
            return GetComponents(entityId, componentType).FirstOrDefault();
        }

        public IComponent GetComponent(uint entityId, IComponent component)
        {
            return GetComponent(entityId, component.GetType());
        }

        public List<IComponent> GetComponents(uint entityId)
        {
            return components.FindAll((component) =>
            {
                return component.EntityId == entityId;
            });
        }

        public List<IComponent> GetComponents(uint entityId, Type componentType)
        {
            return components.FindAll((componentToCheck) =>
            {
                if (componentToCheck == null)
                {
                    return false;
                }
                bool matchingComponent = componentToCheck.GetType() == componentType;
                matchingComponent &= componentToCheck.EntityId == entityId;
                return matchingComponent;
            });
        }

        public List<IComponent> GetComponents(uint entityId, IComponent component)
        {
            return GetComponents(entityId, component.GetType());
        }

        public bool HasComponent(uint entityId, Type component)
        {
            return GetComponent(entityId, component) != null;
        }

        public bool HasComponent(uint entityId, IComponent component)
        {
            return HasComponent(entityId, component.GetType());
        }

        public void RemoveComponents(uint entityId, Type componentType)
        {
            foreach (IComponent removeComponent in GetComponents(entityId, componentType))
            {
                components.Remove(removeComponent);
            }
            eventManager.FireEvent<ComponentRemovedEvent>(this, new ComponentRemovedEvent(entityId));
        }

        public void RemoveComponents(uint entityId, IComponent component)
        {
            RemoveComponents(entityId, component.GetType());
        }

        public void RemoveComponents(uint entityId)
        {
            foreach (IComponent removeComponent in GetComponents(entityId))
            {
                components.Remove(removeComponent);
            }
            eventManager.FireEvent<ComponentRemovedEvent>(this, new ComponentRemovedEvent(entityId));
        }

        public void RemoveEntity(uint entityId)
        {
            RemoveComponents(entityId);
            entities.Remove(entityId);
            eventManager.FireEvent<EntityRemovedEvent>(this, new EntityRemovedEvent(entityId));
            removedEntities.Enqueue(entityId);
        }

        public void EventNotification(object sender, EventArgs eventArgs)
        {
            if (eventArgs is AddEntityEvent)
            {
                uint entityId = CreateEntity();
                AddEntityEvent addEntityEvent = (AddEntityEvent)eventArgs;
                foreach (IComponent gameComponent in addEntityEvent.Components)
                {
                    AddComponent(entityId, gameComponent, false);
                }

                eventManager.FireEvent<NewComponentEvent>(this, new NewComponentEvent(entityId));

                return;
            }

            if (eventArgs is RemoveComponentEvent)
            {
                RemoveComponentEvent componentRemoveEvent = (RemoveComponentEvent) eventArgs;
                RemoveComponents(componentRemoveEvent.EntityId, componentRemoveEvent.ComponentType);
                return;
            }

            if (eventArgs is RemoveEntityEvent)
            {
                RemoveEntityEvent removeEntityEvent = (RemoveEntityEvent)eventArgs;
                RemoveEntity(removeEntityEvent.EntityId);
            }
        }
    }
}

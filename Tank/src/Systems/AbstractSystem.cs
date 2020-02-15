using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.EntityComponentSystem.Validator;
using Tank.src.Events;
using Tank.src.Events.ComponentBased;
using Tank.src.Events.EntityBased;
using Tank.src.Interfaces.EntityComponentSystem;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.src.Systems
{
    abstract class AbstractSystem : ISystem
    {
        protected List<uint> watchedEntities;

        protected List<uint> entitiesToRemove;

        protected IGameEngine gameEngine;

        protected IEntityManager entityManager => gameEngine.EntityManager;

        protected IEventManager eventManager => gameEngine.EventManager;

        protected List<IValidatable> validators;

        public AbstractSystem()
        {
            validators = new List<IValidatable>();
            entitiesToRemove = new List<uint>();
        }

        public virtual void Initialize(IGameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
            watchedEntities = new List<uint>();
            eventManager.SubscribeEvent(this, typeof(NewEntityEvent));
            eventManager.SubscribeEvent(this, typeof(EntityRemovedEvent));
            eventManager.SubscribeEvent(this, typeof(NewComponentEvent));
            eventManager.SubscribeEvent(this, typeof(ComponentRemovedEvent));
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        protected void FireEvent<T>(T args) where T : EventArgs
        {
            eventManager.FireEvent<T>(this, args);
        }

        public virtual void EventNotification(object sender, EventArgs eventArgs)
        {
            NewEntityAdded(eventArgs);
            EntityRemoved(eventArgs);
        }

        protected virtual void NewEntityAdded(EventArgs eventArgs)
        {
            if (eventArgs is EntityBasedEvent)
            {
                EntityBasedEvent entityBasedEvent = (EntityBasedEvent)eventArgs;
                if ((eventArgs is NewEntityEvent))
                {
                    EntityAdded(entityBasedEvent.EntityId);
                    return;
                }
                if ((eventArgs is NewComponentEvent))
                {
                    ComponentAdded(entityBasedEvent.EntityId);
                    return;
                }
            }
        }

        protected virtual void EntityRemoved(EventArgs eventArgs)
        {
            if (eventArgs is EntityBasedEvent)
            {
                EntityBasedEvent entityBasedEvent = (EntityBasedEvent)eventArgs;
                if ((eventArgs is EntityRemovedEvent))
                {
                    EntityRemoved(entityBasedEvent.EntityId);
                    return;
                }

                if (eventArgs is ComponentRemovedEvent)
                {
                    ComponentRemoved(entityBasedEvent.EntityId);
                }
            }

        }

        protected virtual bool EntityIsRelevant(uint entityId)
        {
            foreach (IValidatable validateable in validators)
            {
                if (validateable.IsValidEntity(entityId, entityManager))
                {
                    return true;
                }
            }
            return false;
        }


        public virtual void EntityAdded(uint entityId)
        {
            if (EntityIsRelevant(entityId))
            {
                if (entitiesToRemove.Contains(entityId))
                {
                    entitiesToRemove.Remove(entityId);
                }
                if (watchedEntities.Contains(entityId))
                {
                    return;
                }
                watchedEntities.Add(entityId);
            }
        }

        public virtual void EntityRemoved(uint entityId)
        {
            if (!entitiesToRemove.Contains(entityId))
            {
                entitiesToRemove.Add(entityId);
            }
            
        }

        public virtual void ComponentAdded(uint entityId)
        {
            EntityAdded(entityId);
        }

        public virtual void ComponentRemoved(uint entityId)
        {
            if (!EntityIsRelevant(entityId))
            {
                EntityRemoved(entityId);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = entitiesToRemove.Count - 1; i >= 0; i--)
            {
                watchedEntities.Remove(entitiesToRemove[i]);
                entitiesToRemove.RemoveAt(i);
            }
        }

        
    }
}

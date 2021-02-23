using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.EntityComponentSystem.Validator;
using Tank.Events;
using Tank.Events.ComponentBased;
using Tank.Events.EntityBased;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Wrapper;

namespace Tank.Systems
{
    /// <summary>
    /// This abstract class is a base instance of the system. The class implements the ISystem interface
    /// </summary>
    abstract class AbstractSystem : ISystem
    {
        /// <summary>
        /// Update system is currently locked
        /// </summary>
        protected bool updateLocked;

        /// <summary>
        /// Draw system is currently locked
        /// </summary>
        protected bool drawLocked;

        /// <summary>
        /// A list with all the watched entites for this system
        /// </summary>
        protected List<uint> watchedEntities;

        /// <summary>
        /// A list with all the entities which must be added
        /// </summary>
        protected List<uint> newEntities;

        /// <summary>
        /// A list with all the entites which should be removed on the next cycle
        /// </summary>
        protected List<uint> entitiesToRemove;

        /// <summary>
        /// A reference to the game engine for easier access
        /// </summary>
        protected IGameEngine gameEngine;

        /// <summary>
        /// An easier access to the entity manager
        /// </summary>
        protected IEntityManager entityManager => gameEngine.EntityManager;

        /// <summary>
        /// An easier access to the event manager
        /// </summary>
        protected IEventManager eventManager => gameEngine.EventManager;

        /// <summary>
        /// An easier access to the content wrapper
        /// </summary>
        protected ContentWrapper contentManager => gameEngine.ContentManager;

        /// <summary>
        /// The list of the validators to define if an entity is managed by this system
        /// </summary>
        protected List<IValidatable> validators;

        /// <summary>
        /// Create a new instance for this class
        /// </summary>
        public AbstractSystem()
        {
            validators = new List<IValidatable>();
        }

        /// <summary>
        /// Initialize the system and register to base events
        /// </summary>
        /// <param name="gameEngine">In instance of the game engine needed to use the event and entity manager</param>
        public virtual void Initialize(IGameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
            watchedEntities = new List<uint>();
            entitiesToRemove = new List<uint>();
            newEntities = new List<uint>();

            eventManager.SubscribeEvent(this, typeof(NewEntityEvent));
            eventManager.SubscribeEvent(this, typeof(EntityRemovedEvent));
            eventManager.SubscribeEvent(this, typeof(NewComponentEvent));
            eventManager.SubscribeEvent(this, typeof(ComponentRemovedEvent));
        }

        /// <summary>
        /// Better access to fire an event to the event manager
        /// </summary>
        /// <typeparam name="T">The class of the event to fire</typeparam>
        /// <param name="args">The arguments of the event to fire</param>
        protected void FireEvent<T>(T args) where T : EventArgs
        {
            eventManager.FireEvent(this, args);
        }

        /// <summary>
        /// This method will fetch the basic events from the game
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="eventArgs">The arguments from the event</param>
        public virtual void EventNotification(object sender, EventArgs eventArgs)
        {
            NewEntityAdded(eventArgs);
            EntityRemoved(eventArgs);
        }

        /// <summary>
        /// This method will add new entites to the watch list if valid for this system
        /// </summary>
        /// <param name="eventArgs">The event args from the event</param>
        protected virtual void NewEntityAdded(EventArgs eventArgs)
        {
            if (eventArgs is EntityBasedEvent)
            {
                EntityBasedEvent entityBasedEvent = (EntityBasedEvent)eventArgs;
                if (eventArgs is NewEntityEvent)
                {
                    EntityAdded(entityBasedEvent.EntityId);
                    return;
                }
                if (eventArgs is NewComponentEvent)
                {
                    ComponentAdded(entityBasedEvent.EntityId);
                    return;
                }
            }
        }

        /// <summary>
        /// This method will remove entites from the watch list
        /// </summary>
        /// <param name="eventArgs">The arguments from the event</param>
        protected virtual void EntityRemoved(EventArgs eventArgs)
        {
            if (eventArgs is EntityBasedEvent)
            {
                EntityBasedEvent entityBasedEvent = (EntityBasedEvent)eventArgs;
                if (eventArgs is EntityRemovedEvent)
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

        /// <summary>
        /// This method will check if an entity id is relevant for this system.
        /// It will check against the validateable list
        /// </summary>
        /// <param name="entityId">The id to check if it is valid</param>
        /// <returns>True if the entity is valid for the system</returns>
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

        /// <summary>
        /// This method is getting called if an entity was added
        /// </summary>
        /// <param name="entityId">The id of the entity which got added</param>
        protected virtual void EntityAdded(uint entityId)
        {
            if (EntityIsRelevant(entityId))
            {
                if (entitiesToRemove.Contains(entityId))
                {
                    entitiesToRemove.Remove(entityId);
                }
                if (watchedEntities.Contains(entityId) || newEntities.Contains(entityId))
                {
                    return;
                }
                newEntities.Add(entityId);
            }
        }

        /// <summary>
        /// This method will add an entity to the remove list
        /// </summary>
        /// <param name="entityId">The id to add to the remove list</param>
        protected virtual void EntityRemoved(uint entityId)
        {
            RemoveEntity(entityId);
        }

        /// <summary>
        /// This method will be triggerd if an component was added to an entity
        /// </summary>
        /// <param name="entityId">The id of the entity an component was added to</param>
        protected virtual void ComponentAdded(uint entityId)
        {
            EntityAdded(entityId);
        }

        /// <summary>
        /// This method will be triggerd if a component was removed from an entity
        /// </summary>
        /// <param name="entityId">The id of the entity the component was removed from</param>
        protected virtual void ComponentRemoved(uint entityId)
        {
            if (!EntityIsRelevant(entityId))
            {
                EntityRemoved(entityId);
            }
        }

        /// <summary>
        /// Add an entity for removing in the next cycle
        /// </summary>
        /// <param name="entityId"></param>
        protected virtual void RemoveEntity(uint entityId)
        {
            if (!entitiesToRemove.Contains(entityId))
            {
                entitiesToRemove.Add(entityId);
            }
        }

        protected virtual void DoRemoveEntities()
        {
            if (updateLocked || drawLocked)
            {
                return;
            }
            foreach (uint entityId in entitiesToRemove)
            {
                FireEvent(new RemoveEntityEvent(entityId));
            }
        }

        /// <summary>
        /// This method is the update method of the game.
        /// It will remove all the entites from the watched entites if present in the remove list
        /// </summary>
        /// <param name="gameTime">The gametime from monogame</param>
        public virtual void Update(GameTime gameTime)
        {
            if (updateLocked || drawLocked)
            {
                return;
            }

            for (int i = entitiesToRemove.Count - 1; i >= 0; i--)
            {
                watchedEntities.Remove(entitiesToRemove[i]);
                entitiesToRemove.RemoveAt(i);
            }

            for (int i = newEntities.Count - 1; i >= 0; i--)
            {
                watchedEntities.Add(newEntities[i]);
                newEntities.RemoveAt(i);
            }
        }


        /// <summary>
        /// An basic implementation of the draw method
        /// </summary>
        /// <param name="gameTime">The gametime given from monogame</param>
        public virtual void Draw(GameTime gameTime)
        {
        }
    }
}

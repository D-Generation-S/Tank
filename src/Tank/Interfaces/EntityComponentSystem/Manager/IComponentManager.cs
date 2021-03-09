using System;
using System.Collections.Generic;

namespace Tank.Interfaces.EntityComponentSystem.Manager
{
    interface IComponentManager : IClearable
    {
        /// <summary>
        /// Initialize the entity manager
        /// </summary>
        /// <param name="eventManager">The event manager to use</param>
        public void Initialize(IEventManager eventManager);

        /// <summary>
        /// Get all the entities with a specific component
        /// </summary>
        /// <param name="type">The type of the components to find the entites for</param>
        /// <returns></returns>
        public List<uint> GetEntitiesWithComponent(Type type);

        /// <summary>
        /// This method will allow you to add an component to an existing entity
        /// </summary>
        /// <param name="entityId">The id of the entity to add the component to</param>
        /// <param name="component">The component to add</param>
        /// <returns>Will return true if adding the component was successful</returns>
        bool AddComponent(uint entityId, IComponent component);

        /// <summary>
        /// This method will tell you if a component does exists on a entity
        /// </summary>
        /// <param name="entityId">The id of the entity to check</param>
        /// <param name="componentType">The type of the component to check if it exists on the entity</param>
        /// <returns>Return true if the component is existing</returns>
        bool HasComponent(uint entityId, Type componentType);        
        
        /// <summary>
        /// This method will get you all the components from an entity
        /// </summary>
        /// <param name="entityId">All the components from the given entity id</param>
        /// <returns>A list with the components from this entity</returns>
        List<IComponent> GetComponents(uint entityId);

        /// <summary>
        /// This method will return you all the components from an entity based on an component
        /// </summary>
        /// <param name="entityId">All the components from the given entity id</param>
        /// <param name="componentType">The type of the component to get from the specifig entity</param>
        /// <returns>A list with the components from this entity with a specific type</returns>
        List<IComponent> GetComponents(uint entityId, Type componentType);

        /// <summary>
        /// Get the first component from the entity from a given type
        /// </summary>
        /// <param name="entityId">The id of the entity to search on for the component</param>
        /// <param name="componentType">The type of the component to get</param>
        /// <returns>The component if present on the entity or null</returns>
        IComponent GetComponent(uint entityId, Type componentType);

        /// <summary>
        /// Get the first component from the entity from a given type
        /// </summary>
        /// <typeparam name="T">The component type to get from the entity</typeparam>
        /// <param name="entityId">The id of the entity to search on for the component</param>
        /// <returns>The component if present on the entity or null</returns>
        T GetComponent<T>(uint entityId) where T : IComponent;

        /// <summary>
        /// Move a component from one entity to another
        /// </summary>
        /// <param name="targetEntityId">The id of the target entity</param>
        /// <param name="targetEntityId">The target entity id</param>
        /// <param name="type">The type of the component to move</param>
        /// <returns>True if moving was successful</returns>
        bool MoveComponent(uint sourceEntity, uint targetEntityId, Type type);

        /// <summary>
        /// Move a component from one entity to another
        /// </summary>
        /// <param name="targetEntityId">The id of the target entity</param>
        /// <param name="componentToMove">The component to move</param>
        /// <returns></returns>
        bool MoveComponent(uint targetEntityId, IComponent componentToMove);

        /// <summary>
        /// This method will remove all components from a given type from the entity
        /// </summary>
        /// <param name="entityId">The entity id to remove the components from</param>
        /// <param name="componentType">The type of the component to remove</param>
        void RemoveComponents(uint entityId, Type componentType);

        /// <summary>
        /// Remove all the components from an given entity
        /// </summary>
        /// <param name="entityId">The entity id to remove the components from</param>
        void RemoveComponents(uint entityId);

        /// <summary>
        /// Create a new component
        /// </summary>
        /// <typeparam name="T">The type of component to create</typeparam>
        /// <returns>The new component</returns>
        public T CreateComponent<T>() where T : IComponent;

        /// <summary>
        /// Create a new component and add it to the entity
        /// </summary>
        /// <typeparam name="T">The type of component to create</typeparam>
        /// <param name="entityId">The entity id to add it</param>
        /// <returns>The new component</returns>
        public T CreateComponent<T>(uint entityId) where T : IComponent;

        /// <summary>
        /// The the number of components
        /// </summary>
        /// <returns>The number of component</returns>
        public int GetUsedComponentCount();

        /// <summary>
        /// The the number of components
        /// </summary>
        /// <returns>The number of component</returns>
        public int GetComponentCount();
    }
}

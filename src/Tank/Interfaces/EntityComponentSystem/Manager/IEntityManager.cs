using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Interfaces.EntityComponentSystem.Manager
{
    /// <summary>
    /// This interface describes an entity manager
    /// </summary>
    interface IEntityManager
    {
        /// <summary>
        /// Initialize the manager with the event manager so it can register to events
        /// </summary>
        /// <param name="eventManager"></param>
        void Initialize(IEventManager eventManager);

        /// <summary>
        /// This method will create a new entity
        /// </summary>
        /// <returns>The uint of the newly added entity</returns>
        uint CreateEntity();

        /// <summary>
        /// This method will create a new entity
        /// </summary>
        /// <param name="notifySystems">Should the systems are getting notified</param>
        /// <returns>The uint of the newly added entity</returns>
        uint CreateEntity(bool notifySystems);

        /// <summary>
        /// This method will check if a entity is already existing
        /// </summary>
        /// <param name="entityId">The id of the entity to check</param>
        /// <returns>True if the entity exists</returns>
        bool EntityExists(uint entityId);

        /// <summary>
        /// Get all the entities with a specific component
        /// </summary>
        /// <typeparam name="T">The component type to search for</typeparam>
        /// <returns></returns>
        List<uint> GetEntitiesWithComponent<T>() where T : IComponent;

        /// <summary>
        /// This mehthod will remove a entity from the list and inform the systems
        /// </summary>
        /// <param name="entityId">The id of the entity which should be removed</param>
        void RemoveEntity(uint entityId);

        /// <summary>
        /// This method will allow you to add an component to an existing entity
        /// </summary>
        /// <param name="entityId">The id of the entity to add the component to</param>
        /// <param name="component">The component to add</param>
        /// <returns>Will return true if adding the component was successful</returns>
        bool AddComponent(uint entityId, IComponent component);

        /// <summary>
        /// This method will allow you to add an component to an existing entity
        /// </summary>
        /// <param name="entityId">The id of the entity to add the component to</param>
        /// <param name="component">The component to add</param>
        /// <param name="informSystems">Adding a component should inform the systems</param>
        /// <returns>Will return true if adding the component was successful</returns>
        bool AddComponent(uint entityId, IComponent component, bool informSystems);

        /// <summary>
        /// This method will tell you if a component does exists on a entity
        /// </summary>
        /// <param name="entityId">The id of the entity to check</param>
        /// <param name="component">The component to check if it exists on the entity</param>
        /// <returns>Return true if the component is existing</returns>
        bool HasComponent(uint entityId, IComponent component);

        /// <summary>
        /// This method will tell you if a component does exists on a entity
        /// </summary>
        /// <param name="entityId">The id of the entity to check</param>
        /// <param name="componentType">The typ of the component to check if it exists on the entity</param>
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
        /// <param name="component">The component to get from the entity (By type)</param>
        /// <returns>A list with the components from this entity with a specific type</returns>
        List<IComponent> GetComponents(uint entityId, IComponent component);

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
        /// <param name="component">The component to get (by type)</param>
        /// <returns>The component if present on the entity or null</returns>
        IComponent GetComponent(uint entityId, IComponent component);

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
        /// <param name="componentToMove">The component to move</param>
        /// <returns></returns>
        bool MoveComponent(uint targetEntityId, IComponent componentToMove);

        /// <summary>
        /// Move a component from one entity to another
        /// </summary>
        /// <param name="targetEntityId">The id of the target entity</param>
        /// <param name="componentToMove">The component to move</param>
        /// <returns></returns>
        bool MoveComponent<T>(uint sourceEntity, uint targetEntityId) where T : IComponent;

        /// <summary>
        /// This method will remove all components from a given type from the entity
        /// </summary>
        /// <param name="entityId">The entity id to remove the components from</param>
        /// <param name="componentType">The type of the component to remove</param>
        void RemoveComponents(uint entityId, Type componentType);

        /// <summary>
        /// This method will remove all components from a given type from the entity
        /// </summary>
        /// <param name="entityId">The entity id to remove the components from</param>
        /// <param name="component">The component to remove (By type)</param>
        void RemoveComponents(uint entityId, IComponent component);

        /// <summary>
        /// Remove all the components from an given entity
        /// </summary>
        /// <param name="entityId">The entity id to remove the components from</param>
        void RemoveComponents(uint entityId);
    }
}

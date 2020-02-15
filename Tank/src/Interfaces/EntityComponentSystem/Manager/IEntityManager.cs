using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Interfaces.EntityComponentSystem.Manager
{
    interface IEntityManager
    {
        void Initialize(IEventManager eventManager);

        /// <summary>
        /// This method will create a new entity
        /// </summary>
        /// <returns></returns>
        uint CreateEntity();

        bool EntityExists(uint entityId);

        void RemoveEntity(uint entityId);

        List<IComponent> GetComponents(uint entityId);

        List<IComponent> GetComponents(uint entityId, IComponent component);

        List<IComponent> GetComponents(uint entityId, Type componentType);

        IComponent GetComponent(uint entityId, IComponent component);

        IComponent GetComponent(uint entityId, Type componentType);

        T GetComponent<T>(uint entityId) where T : IComponent;

        bool HasComponent(uint entityId, IComponent component);

        bool HasComponent(uint entityId, Type componentType);

        bool AddComponent(uint entityId, IComponent component);

        void RemoveComponents(uint entityId, Type componentType);

        void RemoveComponents(uint entityId, IComponent component);

        void RemoveComponents(uint entityId);
    }
}

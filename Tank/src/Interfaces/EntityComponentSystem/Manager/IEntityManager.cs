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

        List<IGameComponent> GetComponents(uint entityId);

        List<IGameComponent> GetComponents(uint entityId, IGameComponent component);

        List<IGameComponent> GetComponents(uint entityId, Type componentType);

        IGameComponent GetComponent(uint entityId, IGameComponent component);

        IGameComponent GetComponent(uint entityId, Type componentType);

        T GetComponent<T>(uint entityId) where T : IGameComponent;

        bool HasComponent(uint entityId, IGameComponent component);

        bool HasComponent(uint entityId, Type componentType);

        bool AddComponent(uint entityId, IGameComponent component);

        void RemoveComponents(uint entityId, Type componentType);

        void RemoveComponents(uint entityId, IGameComponent component);

        void RemoveComponents(uint entityId);
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.EntityComponentSystem;
using Tank.src.Interfaces.EntityComponentSystem.Manager;
using Tank.src.Wrapper;

namespace Tank.src.EntityComponentSystem.Manager
{
    class GameEngine : IGameEngine
    {
        private readonly IEventManager eventManager;
        public IEventManager EventManager => eventManager;

        private readonly IEntityManager entityManager;
        public IEntityManager EntityManager => entityManager;

        private readonly ContentWrapper contentManager;
        public ContentWrapper ContentManager => contentManager;

        private readonly List<ISystem> systems;

        public GameEngine(IEventManager eventManager, IEntityManager entityManager, ContentWrapper contentWrapper)
        {
            systems = new List<ISystem>();
            this.eventManager = eventManager;
            entityManager.Initialize(eventManager);
            this.entityManager = entityManager;
            contentManager = contentWrapper;
        }

        public void AddSystem(ISystem systemToAdd)
        {
            systemToAdd.Initialize(this);
            systems.Add(systemToAdd);
        }

        public void Update(GameTime gameTime)
        {
            foreach (ISystem system in systems)
            {
                system.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (ISystem system in systems)
            {
                system.Draw(gameTime);
            }
        }
    }
}

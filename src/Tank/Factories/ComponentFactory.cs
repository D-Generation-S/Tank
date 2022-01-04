using System.Collections.Generic;
using Tank.Interfaces.Builders;
using TankEngine.EntityComponentSystem;
using TankEngine.Factories;

namespace Tank.Factories
{
    class ComponentFactory : IFactory<List<IComponent>>
    {
        private readonly IGameObjectBuilder builder;

        public ComponentFactory(IGameObjectBuilder builder)
        {
            this.builder = builder;
        }

        public List<IComponent> GetNewObject()
        {
            return builder.BuildGameComponents();
        }
    }
}

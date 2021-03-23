using System;
using System.Collections.Generic;
using System.Text;
using Tank.Interfaces.Builders;
using Tank.Interfaces.EntityComponentSystem;

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

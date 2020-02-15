using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.Builders;
using Tank.src.Interfaces.EntityComponentSystem;
using Tank.src.Interfaces.Factories;

namespace Tank.src.Factories
{
    class RandomExplosionFactory : IGameObjectFactory
    {
        private readonly List<IGameObjectBuilder> gameObjecBuilders;
        private Random rnd;

        public RandomExplosionFactory(List<IGameObjectBuilder> gameObjecBuilders)
        {
            this.gameObjecBuilders = gameObjecBuilders;
            rnd = new Random();
        }

        public List<IComponent> GetGameObjects()
        {
            int position = rnd.Next(0, gameObjecBuilders.Count - 1);
            return gameObjecBuilders[position].BuildGameComponents();
        }
    }
}

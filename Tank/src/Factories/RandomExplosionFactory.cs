using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.Builders;
using Tank.src.Interfaces.EntityComponentSystem;
using Tank.src.Interfaces.Factories;
using Tank.src.Interfaces.Randomizer;

namespace Tank.src.Factories
{
    /// <summary>
    /// This class will randomly pick explosions from the builder list provided to the class
    /// </summary>
    class RandomExplosionFactory : IGameObjectFactory
    {
        /// <summary>
        /// All the builders the factory can pick one from
        /// </summary>
        private readonly List<IGameObjectBuilder> gameObjecBuilders;

        /// <summary>
        /// An instance of the randomizer
        /// </summary>
        private IRandomizer randomizer;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="gameObjecBuilders">A list with builders to be used by the factory</param>
        public RandomExplosionFactory(List<IGameObjectBuilder> gameObjecBuilders, IRandomizer randomizer)
        {
            this.gameObjecBuilders = gameObjecBuilders;
            this.randomizer = randomizer;
        }

        /// <summary>
        /// Get all the components from a new explosion
        /// </summary>
        /// <returns>A list of components making up a explosion</returns>
        public List<IComponent> GetGameObjects()
        {
            int position = (int)randomizer.GetNewNumber(0, gameObjecBuilders.Count - 1);
            return gameObjecBuilders[position].BuildGameComponents();
        }
    }
}

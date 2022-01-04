using System.Collections.Generic;
using Tank.Interfaces.Builders;
using TankEngine.EntityComponentSystem;
using TankEngine.Factories;
using TankEngine.Randomizer;

namespace Tank.Factories
{
    /// <summary>
    /// This class will randomly pick explosions from the builder list provided to the class
    /// </summary>
    class RandomEntityBuilderFactory : IFactory<List<IComponent>>
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
        public RandomEntityBuilderFactory(List<IGameObjectBuilder> gameObjecBuilders, IRandomizer randomizer)
        {
            this.gameObjecBuilders = gameObjecBuilders;
            this.randomizer = randomizer;
        }

        /// <inheritdoc/>
        public List<IComponent> GetNewObject()
        {
            if (randomizer == null)
            {
                return new List<IComponent>();
            }
            int position = (int)randomizer.GetNewNumber(0, gameObjecBuilders.Count);
            return gameObjecBuilders[position].BuildGameComponents();
        }
    }
}

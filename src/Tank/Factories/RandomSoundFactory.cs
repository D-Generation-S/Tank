using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Tank.Interfaces.Randomizer;

namespace Tank.Factories
{
    /// <summary>
    /// This class will return you a random sound effect from a pool
    /// </summary>
    class RandomSoundFactory : IFactory<SoundEffect>
    {
        /// <summary>
        /// A list with all the possible sound effects
        /// </summary>
        private readonly List<SoundEffect> soundEffects;

        /// <summary>
        /// The randomizer to use for the sound effects
        /// </summary>
        private readonly IRandomizer randomizer;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="gameObjecBuilders">A list with builders to be used by the factory</param>
        public RandomSoundFactory(List<SoundEffect> possibleEffects, IRandomizer randomizer)
        {
            //this.gameObjecBuilders = gameObjecBuilders;
            soundEffects = possibleEffects;
            this.randomizer = randomizer;
        }

        /// <inheritdoc/>
        public SoundEffect GetNewObject()
        {
            if (soundEffects == null)
            {
                return null;
            }
            int index = (int)randomizer.GetNewNumber(0, soundEffects.Count - 1);
            return soundEffects[index];
        }
    }
}

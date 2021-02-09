﻿using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.Factories;
using Tank.src.Interfaces.Randomizer;

namespace Tank.src.Factories
{
    /// <summary>
    /// This class will return you a random sound effect from a pool
    /// </summary>
    class RandomSoundFactory : ISoundFactory
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
            this.soundEffects = possibleEffects;
            this.randomizer = randomizer;
        }

        /// <inheritdoc/>
        public SoundEffect GetRandomSoundEffect()
        {
            int index = (int)randomizer.GetNewNumber(0, soundEffects.Count - 1);
            return soundEffects[index];
        }
    }
}
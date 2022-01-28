﻿using System;

namespace TankEngine.Randomizer
{
    /// <summary>
    /// An instance of the system randomizer
    /// </summary>
    public class SystemRandomizer : IRandomizer
    {
        /// <summary>
        /// The internal random class to use
        /// </summary>
        private Random random;

        /// <summary>
        /// Create a new instance of this 
        /// </summary>
        public SystemRandomizer()
        {
            Initzialize(DateTime.Now.Millisecond);
        }

        /// <inheritdoc/>
        public void Initzialize(int seed)
        {
            random = new Random(seed);
        }

        /// <inheritdoc/>
        public float GetNewNumber()
        {
            return (float)random.NextDouble();
        }

        /// <inheritdoc/>
        public float GetNewNumber(float minValue, float maxValue)
        {
            return GetNewNumber() * (maxValue - minValue) + minValue;
        }

        /// <inheritdoc/>
        public int GetNewIntNumber()
        {
            return random.Next();
        }

        /// <inheritdoc/>
        public int GetNewIntNumber(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
    }
}
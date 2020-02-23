using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Interfaces.Randomizer
{
    /// <summary>
    /// This interface will represent a randomizer
    /// </summary>
    interface IRandomizer
    {
        /// <summary>
        /// Initzialize the randomizer
        /// </summary>
        /// <param name="seed"></param>
        void Initzialize(int seed);

        /// <summary>
        /// Get a new random number
        /// </summary>
        /// <returns></returns>
        float GetNewNumber();

        /// <summary>
        /// Get a new random number in between
        /// </summary>
        /// <param name="minValue">Lowest possible value for the number</param>
        /// <param name="maxValue">Hightest possible value for the number</param>
        /// <returns></returns>
        float GetNewNumber(float minValue, float maxValue);
    }
}

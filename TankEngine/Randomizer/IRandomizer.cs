namespace Tank.Interfaces.Randomizer
{
    /// <summary>
    /// This interface will represent a randomizer
    /// </summary>
    public interface IRandomizer
    {
        /// <summary>
        /// Initzialize the randomizer
        /// </summary>
        /// <param name="seed"></param>
        void Initzialize(int seed);

        /// <summary>
        /// Get a new random number
        /// </summary>
        /// <returns>A random float number</returns>
        float GetNewNumber();

        /// <summary>
        /// Get a new random number in between
        /// </summary>
        /// <param name="minValue">Lowest possible value for the number</param>
        /// <param name="maxValue">Hightest possible value for the number</param>
        /// <returns>A random float number between bounds</returns>
        float GetNewNumber(float minValue, float maxValue);

        /// <summary>
        /// Get a new random number as int
        /// </summary>
        /// <returns>A random int number</returns>
        int GetNewIntNumber();

        /// <summary>
        /// Get a new random int number between bounds
        /// </summary>
        /// <param name="minValue">Lower bound</param>
        /// <param name="maxValue">Upper bound</param>
        /// <returns>A random int number beweent bounds</returns>
        int GetNewIntNumber(int minValue, int maxValue);
    }
}

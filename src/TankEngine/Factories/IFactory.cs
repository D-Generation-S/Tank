namespace TankEngine.Factories
{
    /// <summary>
    /// A interface for generic factoies
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFactory<T>
    {
        /// <summary>
        /// Create a new object of type T
        /// </summary>
        /// <returns>The newly created objects</returns>
        T GetNewObject();
    }
}

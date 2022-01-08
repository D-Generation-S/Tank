namespace TankEngine.Loottable
{
    /// <summary>
    /// interface to define a lootttable
    /// </summary>
    /// <typeparam name="T">The type of data which can be stored in the loottable</typeparam>
    public interface ILoottable<T>
    {
        /// <summary>
        /// The number of items on the loottable which could be dropped
        /// </summary>
        int ItemCount { get; }

        /// <summary>
        /// Add a new item to the lotttable which can be looted
        /// </summary>
        /// <param name="item">The item to lott</param>
        /// <param name="weight">The chances for the item to be loot</param>
        void AddItem(T item, int weight);

        /// <summary>
        /// Get a item from the lotttable
        /// </summary>
        /// <returns>A item or default(T) return if nothing was drawn</returns>
        T GetItem();

        /// <summary>
        /// Get a item from the lotttable
        /// </summary>
        /// <param name="rollModifier">Modifier for the roll to get more valuable or worse items</param>
        /// <returns>A item or default(T) return if nothing was drawn</returns>
        T GetItem(int rollModifier);
    }
}

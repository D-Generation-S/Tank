using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TankEngine.Randomizer;

namespace TankEngine.Loottable
{
    public class SimpleLoottable<T> : ILoottable<T>
    {
        /// <summary>
        /// Dictionary with the items to draw
        /// </summary>
        private Dictionary<int, T> items;

        /// <summary>
        /// The lookup table with numbers representing the items
        /// </summary>
        private List<int> table;

        /// <summary>
        /// The percentage of empty fields in the table
        /// </summary>
        private readonly int emptyPercentage;

        /// <summary>
        /// The randomizer to use for drawing items
        /// </summary>
        private readonly IRandomizer randomizer;

        /// <inheritdoc>
        public int ItemCount => items.Count - 1;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="emptyPercentage">The percentage of empty fields in the table</param>
        public SimpleLoottable(int emptyPercentage)
            : this(emptyPercentage, null) { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="emptyPercentage">The percentage of empty fields in the table</param>
        /// <param name="randomizer">The randomizer to use for drawing items from the table</param>
        public SimpleLoottable(int emptyPercentage, IRandomizer randomizer)
        {
            table = new List<int>();
            items = new Dictionary<int, T>();
            items.Add(0, default);
            this.emptyPercentage = MathHelper.Clamp(emptyPercentage, 0, 99);
            this.randomizer = randomizer ?? new SystemRandomizer();
        }

        /// <inheritdoc/>
        public void AddItem(T item, int weight)
        {
            int index = items.Keys.Count;
            items.Add(index, item);
            for (int i = 0; i < weight; i++)
            {
                table.Add(index);
            }
            if (emptyPercentage == 0)
            {
                return;
            }
            table.RemoveAll(nullItem => nullItem == 0);
            float itemPercentage = 100 - emptyPercentage;
            float count = table.Count;
            int emptyFieldsToAdd = (int)((count / itemPercentage) * emptyPercentage);
            for (int i = 0; i < emptyFieldsToAdd; i++)
            {
                table.Add(0);
            }
        }

        /// <inheritdoc/>
        public T GetItem()
        {
            return GetItem(0);
        }

        /// <inheritdoc/>
        public T GetItem(int rollModifier)
        {
            int selection = randomizer.GetNewIntNumber(0, table.Count);
            selection += rollModifier;
            selection = selection >= table.Count ? table.Count - 1 : selection;
            int itemNumber = table[selection];
            return items.ContainsKey(itemNumber) ? items[itemNumber] : default;
        }
    }
}

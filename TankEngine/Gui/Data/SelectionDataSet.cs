using System;

namespace Tank.Gui.Data
{
    /// <summary>
    /// A single dataset for the selection element
    /// </summary>
    public class SelectionDataSet
    {
        /// <summary>
        /// The display name to show
        /// </summary>
        public string DisplayText { get; }

        /// <summary>
        /// The real data
        /// </summary>
        private object data;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="displayText">The text to display</param>
        /// <param name="data">The data to show</param>
        public SelectionDataSet(string displayText, object data)
        {
            DisplayText = displayText;
            this.data = data;
        }

        /// <summary>
        /// Get the data set as generic
        /// </summary>
        /// <typeparam name="T">The type to return the data as</typeparam>
        /// <returns>The data as type T or null</returns>
        public T GetData<T>()
        {
            Type t = typeof(T);
            try
            {
                return (T)Convert.ChangeType(data, t);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}

using TankEngine.DataProvider.Converters;
using TankEngine.DataProvider.Loader.String;

namespace TankEngine.DataProvider.Loader
{
    /// <summary>
    /// Class to load json files
    /// </summary>
    /// <typeparam name="T">The type of data to load</typeparam>
    public class JsonDataLoader<T> : DynamicDataLoader<T, string>
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public JsonDataLoader()
            : base(new StreamReaderLoader(), new JsonConverter<T>())
        {
        }

        /// <inheritdoc/>
        public override T LoadData(string fileName)
        {
            fileName = fileName.Trim();
            if (!fileName.EndsWith(".json"))
            {
                fileName += ".json";
            }
            return base.LoadData(fileName);
        }
    }
}

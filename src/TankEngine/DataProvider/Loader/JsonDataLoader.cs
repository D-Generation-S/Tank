using System;
using System.IO;
using System.Text.Json;

namespace TankEngine.DataProvider.Loader
{
    /// <summary>
    /// Class to load json files
    /// </summary>
    /// <typeparam name="T">The type of data to load</typeparam>
    public class JsonDataLoader<T> : AbstractDataLoader<T>
    {
        /// <inheritdoc/>
        public override T LoadData(string fileName)
        {
            T returnData = default(T);
            fileName = fileName.Trim();
            if (!fileName.EndsWith(".json"))
            {
                fileName += ".json";
            }

            if (!File.Exists(fileName))
            {
                return returnData;
            }

            using (StreamReader reader = new StreamReader(fileName))
            {
                try
                {
                    returnData = JsonSerializer.Deserialize<T>(reader.ReadToEnd());
                }
                catch (Exception)
                {
                    return returnData;
                }
            }
            return returnData;
        }
    }
}

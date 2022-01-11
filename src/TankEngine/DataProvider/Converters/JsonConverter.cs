using System;
using System.Text.Json;

namespace TankEngine.DataProvider.Converters
{
    /// <summary>
    /// Class to convert a string data set of type json to a specific object
    /// </summary>
    /// <typeparam name="T">The resulting object</typeparam>
    public class JsonConverter<T> : AbstractConverter<string, T>
    {
        /// <inheritdoc/>
        public override T Convert(string value)
        {
            T returnData = default(T);
            try
            {
                returnData = JsonSerializer.Deserialize<T>(value);
            }
            catch (Exception)
            {
            }
            return returnData;
        }
    }
}

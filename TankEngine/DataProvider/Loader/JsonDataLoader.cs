using System;
using System.IO;
using System.Text.Json;

namespace TankEngine.DataProvider.Loader
{
    public class JsonDataLoader<T> : AbstractDataLoader<T>
    {
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

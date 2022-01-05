using System.IO;
using System.Text.Json;

namespace TankEngine.DataProvider.Saver
{
    /// <summary>
    /// Class to save data as json file
    /// </summary>
    /// <typeparam name="T">The type of data to save</typeparam>
    public class JsonDataSaver<T> : AbstractDataSaver<T>
    {
        /// <inheritdoc/>
        public override bool SaveData(T dataToSave, string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(JsonSerializer.Serialize(dataToSave));
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
    }
}

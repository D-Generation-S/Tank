using System.IO;

namespace TankEngine.DataProvider.Loader.String
{
    /// <summary>
    /// Class to use for loading data from a file on the computer disc as string
    /// </summary>
    public class StreamReaderLoader : AbstractDataLoader<string>
    {
        /// <inheritdoc/>
        public override string LoadData(string fileName)
        {
            string returnString = string.Empty;
            if (!File.Exists(fileName))
            {
                return returnString;
            }

            using (StreamReader reader = new StreamReader(fileName))
            {
                returnString = reader.ReadToEnd();
            }
            return returnString;
        }
    }
}

using System.IO;
using System.Reflection;

namespace TankEngine.DataProvider.Loader.String
{
    /// <summary>
    /// Loader used to 
    /// </summary>
    public class ResourceStringDataLoader : AbstractDataLoader<string>
    {
        /// <summary>
        /// The assembly used for loading the data
        /// </summary>
        Assembly assemblyToUse;

        /// <inheritdoc/>
        public override string LoadData(string fileName)
        {
            assemblyToUse = assemblyToUse ?? Assembly.GetEntryAssembly();
            string returnValue = string.Empty;
            try
            {
                using (Stream assemblyStream = assemblyToUse.GetManifestResourceStream(fileName))
                {
                    using (StreamReader reader = new StreamReader(assemblyStream))
                    {
                        returnValue = reader.ReadToEnd();
                    }
                }
            }
            catch (System.Exception)
            {

            }
            return returnValue;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.IO;
using TankEngine.Music;

namespace Tank.DataManagement.Loader
{
    /// <summary>
    /// Class to load a playlist from a json
    /// </summary>
    class JsonPlaylistLoader : AbstractDataLoader<Playlist>
    {
        /// <summary>
        /// The base path to use
        /// </summary>
        private string basePath;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public JsonPlaylistLoader()
        {
            basePath = GetGameDataFolder();
        }

        /// <inheritdoc/>
        public override Playlist LoadData(string fileName)
        {
            string realFileName = fileName + ".json";
            realFileName = Path.Combine(basePath, realFileName);
            if (!File.Exists(realFileName))
            {
                return default;
            }
            Playlist returnData = null;
            using (StreamReader reader = new StreamReader(realFileName))
            {
                string readData = reader.ReadToEnd();
                try
                {
                    Playlist data = JsonConvert.DeserializeObject<Playlist>(readData);
                    returnData = data;
                }
                catch (Exception)
                {
                    ///Loading did fail returning null
                }

            }
            return returnData;
        }

        /// <inheritdoc/>
        protected override string GetGameDataFolder()
        {
            return Path.Combine(base.GetGameDataFolder(), "Playlists");
        }
    }
}

using Newtonsoft.Json;
using System;
using System.IO;
using Tank.Music;

namespace Tank.DataManagement.Loader
{
    class JsonPlaylistLoader : AbstractDataLoader<Playlist>
    {
        /// <summary>
        /// The base path to use
        /// </summary>
        private string basePath;

        public JsonPlaylistLoader()
        {
            basePath = GetGameDataFolder();
        }

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

        protected override string GetGameDataFolder()
        {
            return Path.Combine(base.GetGameDataFolder(), "Playlists");
        }
    }
}

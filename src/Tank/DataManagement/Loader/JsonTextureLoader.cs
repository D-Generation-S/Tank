using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.IO;
using Tank.DataManagement.Data;
using TankEngine.DataStructures.Spritesheet;

namespace Tank.DataManagement.Loader
{
    /// <summary>
    /// The json based texture loader
    /// </summary>
    class JsonTextureLoader : AbstractDataLoader<SpriteSheet>
    {
        /// <summary>
        /// The base path to use
        /// </summary>
        private readonly string basePath;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public JsonTextureLoader()
        {
            basePath = GetGameDataFolder();
        }

        /// <inheritdoc/>
        public override SpriteSheet LoadData(string fileName)
        {
            string realFileName = fileName + ".json";
            string fileToLoad = Path.Combine(basePath, realFileName);
            SpriteSheet dataToReturn = null;
            if (!File.Exists(fileToLoad))
            {
                return null;
            }
            using (StreamReader reader = new StreamReader(fileToLoad))
            {
                try
                {
                    SpritesheetData data = JsonConvert.DeserializeObject<SpritesheetData>(reader.ReadToEnd());
                    Texture2D texture = contentWrapper.Load<Texture2D>(data.TextureName);
                    dataToReturn = new SpriteSheet(texture, data.SingleImageSize.GetPoint(), data.DistanceBetweenImages, data.Patterns);
                }
                catch (Exception)
                {
                    // Could not load the file, return null
                }
            }

            return dataToReturn;
        }

        /// <inheritdoc/>
        protected override string GetGameDataFolder()
        {
            return Path.Combine(base.GetGameDataFolder(), "Spritesheets");
        }
    }
}

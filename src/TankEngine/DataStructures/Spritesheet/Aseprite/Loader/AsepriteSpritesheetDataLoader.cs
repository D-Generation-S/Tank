using System;
using System.Collections.Generic;
using TankEngine.DataProvider.Loader;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Loader
{
    /// <summary>
    /// Class used to load aseprite spritesheet data
    /// </summary>
    public class AsepriteSpritesheetDataLoader : AbstractDataLoader<ISpritesheetData>
    {
        /// <summary>
        /// Lazy dataloader to use
        /// </summary>
        private readonly Lazy<JsonDataLoader<AsepriteArrayFileData>> dataLoader;

        /// <summary>
        /// Function used to convert loaded data for frames into correct format
        /// </summary>
        private readonly Func<string, List<SpritesheetProperty>> dataToPropertyConversion;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public AsepriteSpritesheetDataLoader()
        {
            dataLoader = new Lazy<JsonDataLoader<AsepriteArrayFileData>>();
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataToPropertyConversion">Method used to convert the data stored per frame property into proper spritesheet properties</param>
        public AsepriteSpritesheetDataLoader(Func<string, List<SpritesheetProperty>> dataToPropertyConversion)
            : this()
        {
            this.dataToPropertyConversion = dataToPropertyConversion;
        }

        /// <inheritdoc/>
        public override ISpritesheetData LoadData(string fileName)
        {
            AsepriteArrayFileData data = dataLoader.Value.LoadData(fileName);
            return dataToPropertyConversion == null ? data?.GetSpritesheet() : data?.GetSpritesheet(dataToPropertyConversion);
        }
    }
}

using System;
using TankEngine.DataProvider.Loader;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Loader
{
    public class AsepriteSpritesheetDataLoader : AbstractDataLoader<ISpritesheetData>
    {
        private readonly Lazy<JsonDataLoader<AsepriteArrayFileData>> dataLoader;
        public AsepriteSpritesheetDataLoader()
        {
            dataLoader = new Lazy<JsonDataLoader<AsepriteArrayFileData>>();
        }

        public override ISpritesheetData LoadData(string fileName)
        {
            AsepriteArrayFileData data = dataLoader.Value.LoadData(fileName);
            return data?.GetSpritesheet();
        }
    }
}

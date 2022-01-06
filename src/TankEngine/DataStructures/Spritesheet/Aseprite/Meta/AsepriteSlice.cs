using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace TankEngine.DataStructures.Spritesheet.Aseprite.Meta
{
    public class AsepriteSlice
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("keys")]
        public List<AsperiteKey> Keys { get; set; }

        public List<SpritesheetArea> GetSpritesheetArea()
        {
            SpritesheetProperty property = new SpritesheetProperty("data", Data);
            return Keys.Select(Key => new SpritesheetArea(Name, new List<SpritesheetProperty>() { property }, Key.Frame, Key.Bounds.GetRectangle()))
                       .ToList();
        }
    }
}

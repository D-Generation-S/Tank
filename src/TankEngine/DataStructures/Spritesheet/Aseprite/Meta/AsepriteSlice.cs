using System;
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

        /// <summary>
        /// Get the area for this slice as a spritesheet area
        /// </summary>
        /// <returns>A list with spritesheet areas</returns>
        public List<SpritesheetArea> GetSpritesheetArea()
        {
            return GetSpritesheetArea(data =>
            {
                if (data.Contains(","))
                {
                    return data.Split(',').AsEnumerable()
                                          .Select(property => GetPropertyFromString(property))
                                          .Where(property => property != null)
                                          .ToList();

                }
                return new List<SpritesheetProperty>() { GetPropertyFromString(data) };
            });
        }

        /// <summary>
        /// Get the area for this slice as a spritesheet area
        /// </summary>
        /// <param name="getProperties">Method to use to convert the data to proper spritesheet properties</param>
        /// <returns>A list with spritesheet areas</returns>
        public List<SpritesheetArea> GetSpritesheetArea(Func<string, List<SpritesheetProperty>> getProperties)
        {
            List<SpritesheetProperty> properties = getProperties(Data);
            return Keys.Select(Key => new SpritesheetArea(Name, properties, Key.Frame, Key.Bounds.GetRectangle()))
                       .ToList();
        }

        /// <summary>
        /// Get the property from a given string
        /// </summary>
        /// <param name="data">The string to convert as property</param>
        /// <returns>A property to use</returns>
        private SpritesheetProperty GetPropertyFromString(string data)
        {
            if (data.Contains("=") && data.Count(character => character == '=') == 1)
            {
                return GetPropertyWithKey(data);
            }
            return GetSimpleProperty(data);
        }

        /// <summary>
        /// Get a simple property where name is identical to value
        /// </summary>
        /// <param name="data">The string to convert to a simple property</param>
        /// <returns>A property to use</returns>
        private SpritesheetProperty GetSimpleProperty(string data)
        {
            return new SpritesheetProperty(data, data);
        }

        /// <summary>
        /// Get a property with a unique key and a assigned value
        /// </summary>
        /// <param name="data">The string to convert to a property with key</param>
        /// <returns>A property to use</returns>
        private SpritesheetProperty GetPropertyWithKey(string data)
        {
            string[] property = data.Split('=');
            if (property.Length == 2)
            {
                return new SpritesheetProperty(property[0], property[1]);
            }
            return null;
        }
    }
}

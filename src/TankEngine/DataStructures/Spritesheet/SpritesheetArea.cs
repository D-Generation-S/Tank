using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TankEngine.DataStructures.Spritesheet
{
    public class SpritesheetArea
    {
        public string Name { get; }

        public List<SpritesheetProperty> Properties { get; }

        public int FrameNumber { get; }

        public Rectangle Area { get; }

        public SpritesheetArea(string name, List<SpritesheetProperty> properties, int frameNumber, Rectangle area)
        {
            Name = name;
            Properties = properties;
            FrameNumber = frameNumber;
            Area = area;
        }

        /// <summary>
        /// Does this spritesheet area contain a special property value
        /// </summary>
        /// <param name="value">The value to check for</param>
        /// <param name="caseSensitive">Should check case sensitive</param>
        /// <returns>True if value exists</returns>
        public bool ContainsPropertyValue(string value, bool caseSensitive)
        {
            value = caseSensitive ? value : value.ToLower();
            return Properties.Any(property => (caseSensitive ? property.Value : property.Value.ToLower()) == value);
        }

        /// <summary>
        /// Does this spritesheet area contain a special property value (Case-Senstive)
        /// </summary>
        /// <param name="value">The value to check for</param>
        /// <returns>True if value exists</returns>
        public bool ContainsPropertyValue(string value)
        {
            return ContainsPropertyValue(value, true);
        }

        /// <summary>
        /// Does this spritesheet area contain a special property name
        /// </summary>
        /// <param name="name">The name of the property to search for</param>
        /// <param name="caseSensitive">Should check case sensitive</param>
        /// <returns>True if the name exists</returns>
        public bool ContainsPropertyName(string name, bool caseSensitive)
        {
            name = caseSensitive ? name : name.ToLower();
            return Properties.Any(property => (caseSensitive ? property.Name : property.Name.ToLower()) == name);
        }

        /// <summary>
        /// Does this spritesheet area contain a special property name (Case-Senstive)
        /// </summary>
        /// <param name="name">The name of the property to search for</param>
        /// <returns>True if the name exists</returns>
        public bool ContainsPropertyName(string name)
        {
            return ContainsPropertyName(name, true);
        }

        /// <summary>
        /// Does this spritesheet area contain a special property
        /// </summary>
        /// <param name="name">The name of the property to search for</param>
        /// <param name="value">The value to check for</param>
        /// <param name="caseSensitive">Should check case sensitive</param>
        /// <returns>True if the name exists</returns>
        public bool ContainsProperty(string name, string value, bool caseSensitive)
        {
            return Properties.Any(property =>
            {
                string pName = caseSensitive ? property.Name : property.Name.ToLower();
                string pValue = caseSensitive ? property.Value : property.Value.ToLower();
                return pName == name && pValue == value;
            });
        }

        /// <summary>
        /// Does this spritesheet area contain a special property  (Case-Senstive)
        /// </summary>
        /// <param name="name">The name of the property to search for</param>
        /// <param name="value">The value to check for</param>
        /// <returns>True if the name exists</returns>
        public bool ContainsProperty(string name, string value)
        {
            return ContainsProperty(name, value, true);
        }
    }
}

using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace TankEngine.DataStructures.Serializeable
{
    /// <summary>
    /// Class to serialize a rectangle
    /// </summary>
    public class SRectangle : SDimension
    {
        /// <summary>
        /// X position of the rectangle
        /// </summary>
        [JsonPropertyName("x")]
        public int X { get; set; }

        /// <summary>
        /// Y position of the rectangle
        /// </summary>
        [JsonPropertyName("y")]
        public int Y { get; set; }

        /// <summary>
        /// Return a new monogame rectangle 
        /// </summary>
        /// <returns>The monogame rectangle</returns>
        public Rectangle GetRectangle()
        {
            return new Rectangle(X, Y, W, H);
        }
    }
}

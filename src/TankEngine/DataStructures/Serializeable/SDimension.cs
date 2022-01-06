using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace TankEngine.DataStructures.Serializeable
{
    /// <summary>
    /// Simple class with two int fields to describe a dimension with Width and Height
    /// </summary>
    public class SDimension
    {
        /// <summary>
        /// Width of the dimension
        /// </summary>
        [JsonPropertyName("w")]
        public int W { get; set; }

        /// <summary>
        /// Height of the dimentsion
        /// </summary>
        [JsonPropertyName("h")]
        public int H { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public SDimension() : this(0, 0) { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="width">The width for the dimension</param>
        /// <param name="height">The height for the dimension</param>
        public SDimension(int width, int height)
        {
            W = width;
            H = height;
        }

        /// <summary>
        /// Get the dimension as a point
        /// </summary>
        /// <returns>A useable point</returns>
        public Point GetPoint()
        {
            return new Point(W, H);
        }
    }
}

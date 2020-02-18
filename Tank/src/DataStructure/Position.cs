using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.DataStructure
{
    public class Position
    {
        /// <summary>
        /// The x coordinate of the position
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The y coordinate of the position
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Create an empty position
        /// </summary>
        public Position()
        {

        }

        /// <summary>
        /// Create a new position class from x and y values
        /// </summary>
        /// <param name="x">The x value of the position</param>
        /// <param name="y">The y value of the position</param>
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Create a new position class from a given vector2
        /// </summary>
        /// <param name="vector2"></param>
        public Position(Vector2 vector2)
        {
            X = (int)vector2.X;
            Y = (int)vector2.X;
        }
    }
}

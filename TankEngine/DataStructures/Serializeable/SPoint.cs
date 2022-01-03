using Microsoft.Xna.Framework;

namespace TankEngine.DataStructures.Serializeable
{
    /// <summary>
    /// This class is a serializeable one for the monogame point
    /// </summary>
    public class SPoint
    {
        /// <summary>
        /// X component of the point
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y component of the point
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public SPoint() : this(0, 0) { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="x">The x component for the class</param>
        /// <param name="y">The y component for the class</param>
        public SPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Create a instance from a monogame point
        /// </summary>
        /// <param name="point">The monogame point to use</param>
        public SPoint(Point point) : this(point.X, point.Y) { }

        /// <summary>
        /// Create a instance from a monogame point
        /// </summary>
        /// <param name="point">The monogame point to use</param>
        public SPoint(Point? point)
        {
            if (point == null)
            {
                X = 0;
                Y = 0;
                return;
            }
            X = point.Value.X;
            Y = point.Value.Y;
        }

        public Point GetPoint()
        {
            return new Point(X, Y);
        }
    }
}

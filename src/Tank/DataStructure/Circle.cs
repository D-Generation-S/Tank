using Microsoft.Xna.Framework;
using System;

namespace Tank.DataStructure
{
    /// <summary>
    /// This class represents a circly
    /// </summary>
    class Circle
    {
        /// <summary>
        /// The center position of the circle
        /// </summary>
        private Vector2 center;

        /// <summary>
        /// Public accessor of the circle position
        /// </summary>
        public Vector2 Center
        {
            get => center;
            set => center = value;
        }

        /// <summary>
        /// Circle radius
        /// </summary>
        private readonly int radius;

        /// <summary>
        /// Readonly access to the circle radius
        /// </summary>
        public int Radius => radius;

        /// <summary>
        /// Circle diameter
        /// </summary>
        private readonly int diameter;

        /// <summary>
        /// Readonly access to the cirle diameter
        /// </summary>
        public int Diameter => diameter;

        /// <summary>
        /// Create a new circle instance
        /// </summary>
        /// <param name="x">The x position of the circle</param>
        /// <param name="y">The y position of the circle</param>
        /// <param name="radius">The radius for the circle</param>
        public Circle(int x, int y, int radius)
            : this(new Position(x, y), radius)
        {
        }

        /// <summary>
        /// Create a new circle instance
        /// </summary>
        /// <param name="position">The position of the circle</param>
        /// <param name="radius">The radius for the circle</param>
        public Circle(Position position, int radius)
            : this(new Vector2(position.X, position.Y), radius)
        {
        }

        /// <summary>
        /// Create a new circle instance
        /// </summary>
        /// <param name="position">The position of the circle</param>
        /// <param name="radius">The radius for the circle</param>
        public Circle(Vector2 position, int radius)
        {
            center = position;
            this.radius = radius;
            diameter = radius * 2;
        }

        /// <summary>
        /// Is this position inside of the circle
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <returns>Returns true if the position is in the circle</returns>
        public bool IsInInCircle(Position position)
        {
            return IsInInCircle(position.X, position.Y);
        }

        /// <summary>
        /// Is this position inside of the circle
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <returns>Returns true if the position is in the circle</returns>
        public bool IsInInCircle(Point position)
        {
            return IsInInCircle(position.X, position.Y);
        }

        /// <summary>
        /// Is this position inside of the circle
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <returns>Returns true if the position is in the circle</returns>
        public bool IsInInCircle(Vector2 position)
        {
            Vector2 distance = center - position;
            return distance.Length() < radius;
        }

        /// <summary>
        /// Is this position inside of the circle
        /// </summary>
        /// <param name="x">The x position to check</param>
        /// <param name="y">The y position to check</param>
        /// <returns>Returns true if x and y is inside of the circle</returns>
        public bool IsInInCircle(int x, int y)
        {
            Vector2 position = Vector2.UnitX * x + Vector2.UnitY * y;
            return IsInInCircle(position);
        }
    }
}


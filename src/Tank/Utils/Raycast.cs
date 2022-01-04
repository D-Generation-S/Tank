using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Tank.Utils
{
    /// <summary>
    /// This class will create a raycast
    /// </summary>
    class Raycast
    {
        /// <summary>
        /// The origin of the cast
        /// </summary>
        private Vector2 origin;

        /// <summary>
        /// The direction of the cast
        /// </summary>
        private Vector2 direction;

        /// <summary>
        /// The magnitude of the cast
        /// </summary>
        private float magnitude;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="origin">The origin of this cast</param>
        /// <param name="direction">The direction of the cast</param>
        public Raycast(Vector2 origin, Vector2 direction) : this(origin, direction, -1)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="origin">The origin of this cast</param>
        /// <param name="direction">The direction of the cast</param>
        /// <param name="magnitude">The magnitude of the cast</param>
        public Raycast(Vector2 origin, Vector2 direction, float magnitude)
        {
            this.origin = origin;
            this.origin.Round();
            this.direction = direction;
            this.direction.Normalize();
            this.magnitude = magnitude < 0 ? 1 : magnitude;
        }

        /// <summary>
        /// Get the origint
        /// </summary>
        /// <returns>The origin point</returns>
        public Vector2 getOrigin()
        {
            return origin;
        }

        /// <summary>
        /// The direction of the cast
        /// </summary>
        /// <returns>The direction</returns>
        public Vector2 getDirection()
        {
            return direction;
        }

        /// <summary>
        /// All the points between origin and magnitude on direction
        /// </summary>
        /// <returns>A list with points</returns>
        public Point[] GetPoints()
        {
            List<Point> positions = new List<Point>();
            Vector2 startPosition = origin;
            int steps = (int)Math.Round(magnitude) + 1;
            for (int step = 0; step < steps; step++)
            {
                positions.Add(new Point((int)startPosition.X, (int)startPosition.Y));
                startPosition += direction;
            }
            return positions.ToArray();
        }
    }
}

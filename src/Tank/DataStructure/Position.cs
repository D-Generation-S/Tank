using Microsoft.Xna.Framework;

namespace Tank.DataStructure
{
    /// <summary>
    /// This class represents a position on the screen
    /// </summary>   
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
            Y = (int)vector2.Y;
        }

        /// <summary>
        /// Return the position as vector 2
        /// </summary>
        /// <returns>A valid Vector2</returns>
        public Vector2 GetVector2()
        {
            return Vector2.UnitX * X + Vector2.UnitY * Y;
        }

        /// <summary>
        /// Add a position to this position
        /// </summary>
        /// <param name="position">The position to add</param>
        public void Add(Position position)
        {
            Add(position.GetVector2());
        }

        /// <summary>
        /// Add a vector to this position
        /// </summary>
        /// <param name="position">The vector to add</param>
        public void Add(Vector2 position)
        {
            X += (int)position.X;
            Y += (int)position.Y;
        }

        /// <summary>
        /// The plus operant for this object
        /// </summary>
        /// <param name="positionA">The first position</param>
        /// <param name="positionB">The second position</param>
        /// <returns>The position after the calculation</returns>
        public static Position operator+ (Position positionA, Position positionB) {
            return new Position(positionA.X + positionB.X, positionA.Y + positionB.Y);
        }

        /// <summary>
        /// The minus operant for this object
        /// </summary>
        /// <param name="positionA">The first position</param>
        /// <param name="positionB">The second position</param>
        /// <returns>The position after the calculation</returns>
        public static Position operator- (Position positionA, Position positionB)
        {
            return new Position(positionA.X - positionB.X, positionA.Y - positionB.Y);
        }

        /// <summary>
        /// The multiplication operation for this object
        /// </summary>
        /// <param name="positionA">The first position</param>
        /// <param name="positionB">The second position</param>
        /// <returns>The position after the calculation</returns>
        public static Position operator* (Position positionA, Position positionB)
        {
            return new Position(positionA.X * positionB.X, positionA.Y * positionB.Y);
        }

        /// <summary>
        /// The division operant for this object
        /// </summary>
        /// <param name="positionA">The first position</param>
        /// <param name="positionB">The second position</param>
        /// <returns>The position after the calculation</returns>
        public static Position operator/ (Position positionA, Position positionB)
        {
            return new Position(positionA.X / positionB.X, positionA.Y / positionB.Y);
        }
    }
}

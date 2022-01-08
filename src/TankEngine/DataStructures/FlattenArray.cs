using Microsoft.Xna.Framework;
using System;

namespace TankEngine.DataStructures
{
    /// <summary>
    /// A helper class for creating flatten arrays and providing easier access methods
    /// </summary>
    /// <typeparam name="T">The array content to store in this helper class</typeparam>
    public class FlattenArray<T>
    {
        /// <summary>
        /// The width of the old 2D array
        /// </summary>
        private readonly int width;

        /// <summary>
        /// The flatten arrray
        /// </summary>
        private readonly T[] array;

        /// <summary>
        /// Public access to the flatten array
        /// </summary>
        public T[] Array => array;

        /// <summary>
        /// Simple constructor for an empty dataset
        /// </summary>
        /// <param name="width">The width of the 2D array</param>
        /// <param name="height">The height of the 2D array</param>
        public FlattenArray(int width, int height)
        {
            array = new T[width * height];
            this.width = width;
        }

        /// <summary>
        /// Simple constructor for an empty dataset
        /// </summary>
        /// <param name="width">The width of the 2D array</param>
        /// <param name="height">The height of the 2D array</param>
        /// <param name="initialMethod">Method to prefill the array</param>
        public FlattenArray(int width, int height, Func<T> initialMethod)
        {
            array = new T[width * height];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = initialMethod();
            }
            this.width = width;
        }

        /// <summary>
        /// Converts an array to a flatten one
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <param name="singleRowWidth">Size of a single row</param>
        public FlattenArray(T[] array, int singleRowWidth)
        {
            this.array = new T[array.Length];
            width = singleRowWidth;
            for (int x = 0; x < array.Length; x++)
            {
                this.array[x] = array[x];
            }
        }

        /// <summary>
        /// Converts a 2D array to a flatten one
        /// </summary>
        /// <param name="array"></param>
        public FlattenArray(T[,] array)
            : this(array.GetLength(0), array.GetLength(1))
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    SetValue(x, y, array[x, y]);
                }
            }
        }

        /// <summary>
        /// Get the value for a given position
        /// </summary>
        /// <param name="point">The position to get the value for</param>
        /// <returns>The value T at the given position</returns>
        public T GetValue(Point point)
        {
            return GetValue(point.X, point.Y);
        }

        /// <summary>
        /// Get the value for a given position
        /// </summary>
        /// <param name="x">Position x to get the value from</param>
        /// <param name="y">Position y to get the value from</param>
        /// <returns></returns>
        public T GetValue(int x, int y)
        {
            if (!IsInArray(x, y))
            {
                return default;
            }
            return array[GetFlattenPosition(x, y)];
        }

        /// <summary>
        /// Get the position inside of the flatten array
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <returns>The int number of the index in the flatten array</returns>
        private int GetFlattenPosition(int x, int y)
        {
            return y * width + x;
        }

        /// <summary>
        /// Get a part of the flatten array as a new small flatten array
        /// </summary>
        /// <param name="rectangle">The area of the grid to get</param>
        /// <returns>The part of the array if start and end coordinates are in the grid otherwise null</returns>
        public FlattenArray<T> GetArea(Rectangle rectangle)
        {
            return GetArea(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
        }

        /// <summary>
        /// Get a part of the flatten array as a new small flatten array
        /// </summary>
        /// <param name="x">The x start position</param>
        /// <param name="y">The y start position</param>
        /// <param name="endX">The x end position</param>
        /// <param name="endY">The y end position</param>
        /// <returns>The part of the array if start and end coordinates are in the grid otherwise null</returns>
        public FlattenArray<T> GetArea(int x, int y, int endX, int endY)
        {
            int indexEndX = endX - 1;
            int indexEndY = endY - 1;
            if (!IsInArray(x, y) || !IsInArray(indexEndX, indexEndY))
            {
                return default;
            }

            FlattenArray<T> returnArray = new FlattenArray<T>(endX - x, endY - y);
            for (int row = x; row < endX; row++)
            {
                for (int column = y; column < endY; column++)
                {
                    T dataToSet = GetValue(row, column);
                    returnArray.SetValue(row - x, column - y, dataToSet);
                }
            }
            return returnArray;
        }

        /// <summary>
        /// Set the value for a given position
        /// </summary>
        /// <param name="position">The position to set the value for</param>
        /// <param name="value">The value to set</param>
        /// <returns></returns>
        public bool SetValue(Point position, T value)
        {
            return SetValue(position.X, position.Y, value);
        }

        /// <summary>
        /// Set the value for a given position
        /// </summary>
        /// <param name="x">The x position to set the value to</param>
        /// <param name="y">The y position to set the value to</param>
        /// <param name="value">The value to set</param>
        /// <returns></returns>
        public bool SetValue(int x, int y, T value)
        {
            if (!IsInArray(x, y))
            {
                return false;
            }
            array[GetFlattenPosition(x, y)] = value;

            return true;
        }

        /// <summary>
        /// Check if a point is part of the array
        /// </summary>
        /// <param name="x">The x position on the array</param>
        /// <param name="y">The y position on the array</param>
        /// <returns>True if the position is on the array</returns>
        public bool IsInArray(int x, int y)
        {
            return IsInArray(GetFlattenPosition(x, y));
        }

        /// <summary>
        /// Check if a point is part of the array
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <returns>True if the position is on the array</returns>
        public bool IsInArray(Point point)
        {
            return IsInArray(point.X, point.Y);
        }

        /// <summary>
        /// Check if a point is part of the array
        /// </summary>
        /// <param name="vector">The vector to check</param>
        /// <returns>True if the position is on the array</returns>
        public bool IsInArray(Vector2 vector)
        {
            return IsInArray((int)vector.X, (int)vector.Y);
        }

        /// <summary>
        /// Check if a point is part of the array
        /// </summary>
        /// <param name="targetPosition">The position on the array</param>
        /// <returns>True if the position is on the array</returns>
        public bool IsInArray(int targetPosition)
        {
            return targetPosition < array.Length && targetPosition >= 0;
        }
    }
}

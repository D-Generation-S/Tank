using Microsoft.Xna.Framework;
using System;

namespace Tank.DataStructure
{
    /// <summary>
    /// A helper class for creating flatten arrays and providing easier access methods
    /// </summary>
    /// <typeparam name="T">The array content to store in this helper class</typeparam>
    class FlattenArray<T>
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
            for(int i = 0; i < array.Length; i++)
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
        /// <param name="position">The position to get the value for</param>
        /// <returns>The value T at the given position</returns>
        [Obsolete("Please use the method which requires a point")]
        public T GetValue(Position position)
        {
            return GetValue(position.X, position.Y);
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
            int targetPosition = y * width + x;
            if (targetPosition > array.Length || targetPosition < 0)
            {
                return default;
            }

            return array[targetPosition];
        }

        /// <summary>
        /// Set the value for a given position
        /// </summary>
        /// <param name="position">The position to set the value for</param>
        /// <param name="value">The value to set</param>
        /// <returns></returns>
        public bool SetValue(Position position, T value)
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
            int targetPosition = y * width + x;
            if (targetPosition >= array.Length || targetPosition < 0)
            {
                return false;
            }
            array[targetPosition] = value;

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
            return IsInArray(y * width + x);
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
        /// <param name="targetPosition">The position on the array</param>
        /// <returns>True if the position is on the array</returns>
        public bool IsInArray(int targetPosition)
        {
            return targetPosition >= array.Length || targetPosition < 0;
        }
    }
}

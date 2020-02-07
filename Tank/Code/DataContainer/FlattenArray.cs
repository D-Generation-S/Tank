using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Code.DataContainer
{
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
            //this.height = height;
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

        public T GetValue(Position position)
        {
            return GetValue(position.X, position.Y);
        }

        public T GetValue(int x, int y)
        {
            int targetPosition = y * width + x;
            if (targetPosition > array.Length)
            {
                return default(T);
            }

            return array[targetPosition];
        }

        public bool SetValue(Position position, T value)
        {
            return SetValue(position.X, position.Y, value);
        }

        public bool SetValue(int X, int Y, T value)
        {
            int targetPosition = Y * width + X;
            if (targetPosition > array.Length)
            {
                return false;
            }
            array[targetPosition] = value;

            return true;
        }
    }
}

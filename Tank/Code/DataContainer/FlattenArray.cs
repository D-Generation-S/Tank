using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Code.DataContainer
{
    class FlattenArray<T>
    {
        private readonly int width;
        //private readonly int height;

        private readonly T[] array;
        public T[] Array => array;

        public FlattenArray(int width, int height)
        {
            array = new T[width * height];
            this.width = width;
            //this.height = height;
        }

        public FlattenArray(T[][] array)
            : this(array.GetLength(0), array.GetLength(1))
        {
        }

        public T GetValue(Position position)
        {
            return GetValue(position.X, position.Y);
        }

        public T GetValue(int X, int Y)
        {
            int targetPosition = Y * width + X;
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

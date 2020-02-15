using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;

namespace Tank.src.DataStructure
{
    class Circle
    {
        private Position center;
        public Position Center
        {
            get => center;
            set => center = value;
        }

        private readonly int radius;
        public int Radius => radius;

        private readonly int diameter;
        public int Diameter => diameter;

        public Circle(int x, int y, int radius)
            : this (new Position(x, y), radius)
        {
        }

        public Circle(Position position, int radius)
        {
            center = position;
            this.radius = radius;
            diameter = radius * 2;
        }

        public bool IsInInCircle(Position position)
        {
            return IsInInCircle(position.X, position.Y);
        }

        public bool IsInInCircle(int x, int y)
        {
            float TestValue = (float)Math.Sqrt(Math.Pow((x - center.X), 2) + Math.Pow((y - center.Y), 2));
            return TestValue < Radius;
        }
    }
}

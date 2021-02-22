using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataStructure;

namespace Tank.Utils
{
    class Raycast
    {
        private Vector2 origin;

        private Vector2 direction;

        private float magnitude;

        //private 

        public Raycast(Vector2 origin, Vector2 direction) : this(origin, direction, -1)
        {
        }

        public Raycast(Vector2 origin, Vector2 direction, float magnitude)
        {
            this.origin = origin;
            this.origin.Round();
            this.direction = direction;
            this.direction.Normalize();
            this.magnitude = magnitude < 0 ? 1 : magnitude;
        }

        public Vector2 getOrigin()
        {
            return origin;
        }

        public Vector2 getDirection()
        {
            return direction;
        }

        public Position[] GetPositions()
        {
            List<Position> positions = new List<Position>();
            Vector2 startPosition = origin;
            int steps = (int)Math.Round(magnitude) + 1;
            for (int step = 0; step < steps; step++)
            {
                positions.Add(new Position(startPosition));
                startPosition += direction;
            }
            return positions.ToArray();
        }
    }
}

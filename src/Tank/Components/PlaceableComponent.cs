using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Components
{
    /// <summary>
    /// Allow the entity to be placed in the world
    /// </summary>
    class PlaceableComponent : BaseComponent
    {
        /// <summary>
        /// The position of the entity in the world
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Public access to the position of the entity in the world
        /// </summary>
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// The rotation of the entity
        /// </summary>
        private float rotation;

        /// <summary>
        /// Public access to the rotation of the entity
        /// </summary>
        public float Rotation
        {
            get => rotation;
            set => rotation = value;
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public PlaceableComponent() : this(0, 0)
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        public PlaceableComponent(float x, float y) : this(new Vector2(x, y))
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position</param>
        public PlaceableComponent(Vector2 position)
        {
            this.position = position;
        }
    }
}

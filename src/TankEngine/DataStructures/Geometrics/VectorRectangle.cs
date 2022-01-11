using Microsoft.Xna.Framework;

namespace TankEngine.DataStructures.Geometrics
{
    /// <summary>
    /// A rectangle based on vectors
    /// </summary>
    public class VectorRectangle
    {
        /// <summary>
        /// Upper left point of the rectangle
        /// </summary>
        public Vector2 Location;

        /// <summary>
        /// Point on the bottom right
        /// </summary>
        public Vector2 BottomRight => Location + boundingBox;

        /// <summary>
        /// Public readonly access to the size of the rectangle
        /// </summary>
        public Vector2 BoundingBox => boundingBox;

        /// <summary>
        /// Size of the rectangle
        /// </summary>
        private Vector2 boundingBox;

        /// <summary>
        /// The width of the rectangle
        /// </summary>
        public float Width
        {
            get => boundingBox.X;
            set => boundingBox.X = value;
        }

        /// <summary>
        /// The height of the rectangle
        /// </summary>
        public float Height
        {
            get => boundingBox.Y;
            set => boundingBox.Y = value;
        }

        /// <summary>
        /// The x location of the rectangle
        /// </summary>
        public float X
        {
            get => Location.X;
            set => Location.X = value;
        }

        /// <summary>
        /// The y location of the rectangle
        /// </summary>
        public float Y
        {
            get => Location.Y;
            set => Location.Y = value;
        }

        /// <summary>
        /// Location for the right wall of the rectangle
        /// </summary>
        public float Right => Location.X + boundingBox.X;

        /// <summary>
        /// Locataion of the bottom wall of the rectangle
        /// </summary>
        public float Bottom => Location.Y + boundingBox.Y;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="rectangle">The normal rectangle to use</param>
        public VectorRectangle(Rectangle rectangle)
            : this(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position of the rectangle</param>
        /// <param name="size">The size of the rectangle</param>
        public VectorRectangle(Vector2 position, Point size)
            : this(position, new Vector2(size.X, size.Y))
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position of the rectangle</param>
        /// <param name="size">The size of the rectangle</param>
        public VectorRectangle(Vector2 position, float size)
            : this(position, new Vector2(size, size))
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="x">The x location of the rectangle</param>
        /// <param name="y">The y location of the rectangle</param>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        public VectorRectangle(float x, float y, float width, float height)
            : this(new Vector2(x, y), new Vector2(width, height))
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position of the rectangle</param>
        /// <param name="size">The size of the rectangle</param>
        public VectorRectangle(Vector2 position, Vector2 size)
        {
            Location = position;
            boundingBox = size;
        }

        /// <summary>
        /// Does the rectangle contain a speficific point
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <returns></returns>
        public bool Contains(Vector2 position)
        {
            bool contains = X <= position.X;
            contains &= Y <= position.Y;
            contains &= Right >= position.X;
            contains &= Bottom >= position.Y;
            return contains;
        }

        /// <summary>
        /// Is this rectangle intersecting with another rectangle
        /// </summary>
        /// <param name="rectangle">The rectangle to check</param>
        /// <returns>True if intersecting</returns>
        public bool Intersects(VectorRectangle rectangle)
        {
            if (X >= rectangle.BottomRight.X || rectangle.X >= BottomRight.X)
            {
                return false;
            }

            if (Y >= rectangle.BottomRight.Y || rectangle.Y >= BottomRight.Y)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Is this rectangle intersecting with another rectangle
        /// </summary>
        /// <param name="rectangle">The rectangle to check</param>
        /// <returns>True if intersecting</returns>
        public bool Intersects(Rectangle rectangle)
        {
            return Intersects(new VectorRectangle(rectangle));
        }
    }
}

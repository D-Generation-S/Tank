using Microsoft.Xna.Framework;
using System;

namespace Tank.DataStructure.Quadtree
{
    /// <summary>
    /// Data for the quad tree
    /// </summary>
    public class QuadTreeData
    {
        /// <summary>
        /// The position of this dataset
        /// </summary>
        public Vector2 Position { get; private set; }

        /// <summary>
        /// The additional stored data
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public QuadTreeData()
            : this(Vector2.Zero, null)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position for this dataset</param>
        public QuadTreeData(Vector2 position)
            : this(position, null)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position for this dataset</param>
        /// <param name="data">The data for this dataset</param>
        public QuadTreeData(Vector2 position, object data)
        {
            Init(position, data);
        }

        /// <summary>
        /// Init the datastructure again
        /// </summary>
        /// <param name="position">The position to use</param>
        /// <param name="data">The data to store</param>
        public void Init(Vector2 position)
        {
            Init(position, null);
        }

        /// <summary>
        /// Init the datastructure again
        /// </summary>
        /// <param name="position">The position to use</param>
        /// <param name="data">The data to store</param>
        public void Init(Vector2 position, object data)
        {
            Position = position;
            Data = data;
        }

        /// <summary>
        /// Get the data as specific data type
        /// </summary>
        /// <typeparam name="T">The data type to get</typeparam>
        /// <returns></returns>
        public T GetData<T>()
        {
            Type type = typeof(T);
            if (Data.GetType() != type)
            {
                return default(T);
            }
            return (T)Convert.ChangeType(Data, type);
        }
    }
}

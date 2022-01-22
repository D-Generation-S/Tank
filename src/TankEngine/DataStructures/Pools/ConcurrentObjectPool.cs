using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TankEngine.DataStructures.Pools
{
    /// <summary>
    /// A objects pool based on the concurrent Bat
    /// </summary>
    /// <typeparam name="T">The type of items to generate</typeparam>
    public class ConcurrentObjectPool<T> : IObjectPool<T>
    {
        /// <summary>
        /// A list with all the objects
        /// </summary>
        private readonly ConcurrentBag<T> objects;

        /// <summary>
        /// A function to generate new objects
        /// </summary>
        private readonly Func<T> objectGenerator;

        /// <summary>
        /// Create a new instance of this pool with prefilled entries
        /// </summary>
        /// <param name="objectGenerator">The object generator to use</param>
        public ConcurrentObjectPool(Func<T> objectGenerator)
        {
            this.objectGenerator = objectGenerator;
            objects = new ConcurrentBag<T>();
        }

        /// <summary>
        /// Create a new instance of this pool with prefilled entries
        /// </summary>
        /// <param name="objectGenerator">The object generator to use</param>
        /// <param name="startCount">The number of objects to start in the pool</param>

        public ConcurrentObjectPool(Func<T> objectGenerator, int startCount) : this(objectGenerator)
        {
            List<T> createdObjects = new List<T>();
            for (int i = 0; i < startCount; i++)
            {
                createdObjects.Add(Get());
            }
            foreach (T item in createdObjects)
            {
                Return(item);
            }
        }

        /// <inheritdoc/>
        public T Get()
        {
            return objects.TryTake(out T item) ? item : objectGenerator();
        }

        /// <inheritdoc/>
        public void Return(T item) => objects.Add(item);
    }
}

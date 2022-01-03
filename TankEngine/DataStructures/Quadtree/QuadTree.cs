using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.DataStructure.Geometrics;

namespace Tank.DataStructure.Quadtree
{
    /// <summary>
    /// Quadtree to use
    /// </summary>
    public class QuadTree
    {
        /// <summary>
        /// The bounding box for the quad tree
        /// </summary>
        private VectorRectangle boundingBox;

        /// <summary>
        /// The stored data in this quad tree
        /// </summary>
        private QuadTreeData[] data;

        /// <summary>
        /// The capacity before creating sub nodes
        /// </summary>
        private uint capacity;

        /// <summary>
        /// The current field to write into
        /// </summary>
        private uint currentField;

        /// <summary>
        /// Is this quadtree already subdevided
        /// </summary>
        private bool subDivided;

        /// <summary>
        /// The upper left quad tree
        /// </summary>
        private QuadTree upperLeft;

        /// <summary>
        /// The upper right quad tree
        /// </summary>
        private QuadTree upperRight;

        /// <summary>
        /// The lower left quad tree
        /// </summary>
        private QuadTree lowerLeft;

        /// <summary>
        /// The lower right quad tree
        /// </summary>
        private QuadTree lowerRight;

        /// <summary>
        /// Create a new instance of this class with capacity of 4
        /// </summary>
        /// <param name="boundingBox">The bounding box to use</param>
        public QuadTree(VectorRectangle boundingBox)
            : this(boundingBox, 4)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="boundingBox">The bounding box to use</param>
        /// <param name="capacity">The capacity to use</param>
        public QuadTree(VectorRectangle boundingBox, uint capacity)
        {
            this.capacity = capacity;
            data = new QuadTreeData[capacity];
            this.boundingBox = boundingBox;
            Clear();
        }

        /// <summary>
        /// Clear the tree
        /// </summary>
        public void Clear()
        {
            currentField = 0;
            Array.Clear(data, 0, data.Length);
            if (subDivided)
            {
                upperLeft.Clear();
                upperRight.Clear();
                lowerLeft.Clear();
                lowerRight.Clear();
            }
            subDivided = false;
        }

        /// <summary>
        /// Insert data to the tree
        /// </summary>
        public bool Insert(QuadTreeData point)
        {
            if (!boundingBox.Contains(point.Position))
            {
                return false;
            }

            if (currentField < capacity)
            {
                data[currentField] = point;
                currentField++;
                return true;
            }
            if (!subDivided)
            {
                SubDivide();
            }
            if (upperLeft.Insert(point))
            {
                return true;
            }
            if (upperRight.Insert(point))
            {
                return true;
            }
            if (lowerLeft.Insert(point))
            {
                return true;
            }
            if (lowerRight.Insert(point))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Query the tree for an area
        /// </summary>
        public List<QuadTreeData> Query(VectorRectangle boundingArea)
        {
            List<QuadTreeData> returnData = new List<QuadTreeData>();

            if (!boundingArea.Intersects(boundingBox))
            {
                return returnData;
            }

            for (int i = 0; i < currentField; i++)
            {
                returnData.Add(data[i]);
            }

            if (subDivided)
            {
                returnData.AddRange(upperLeft.Query(boundingArea));
                returnData.AddRange(upperRight.Query(boundingArea));
                returnData.AddRange(lowerLeft.Query(boundingArea));
                returnData.AddRange(lowerRight.Query(boundingArea));
            }

            return returnData;
        }

        /// <summary>
        /// Query the tree for an area
        /// </summary>
        public void Query(Rectangle rectangle)
        {

        }

        public uint GetPointCount()
        {
            uint points = currentField;
            if (subDivided)
            {
                points += upperLeft.GetPointCount();
                points += upperRight.GetPointCount();
                points += lowerLeft.GetPointCount();
                points += lowerRight.GetPointCount();
            }

            return points;
        }

        /// <summary>
        /// Subdivide this quadtree
        /// </summary>
        private void SubDivide()
        {
            subDivided = true;
            float halfWidth = boundingBox.Width / 2;
            float halfHeight = boundingBox.Height / 2;
            upperLeft = new QuadTree(
                new VectorRectangle(
                    boundingBox.Location.X,
                    boundingBox.Location.Y,
                    halfWidth,
                    halfHeight
                    ),
                capacity
                );
            upperRight = new QuadTree(
                new VectorRectangle(
                    boundingBox.Location.X + halfWidth,
                    boundingBox.Y,
                    halfWidth,
                    halfHeight
                    ),
                capacity
                );
            lowerLeft = new QuadTree(
                new VectorRectangle(
                    boundingBox.Location.X,
                    boundingBox.Y + halfHeight,
                    halfWidth,
                    halfHeight
                    ),
                capacity
                );
            lowerRight = new QuadTree(
                new VectorRectangle(
                    boundingBox.X + halfWidth,
                    boundingBox.Y + halfHeight,
                    halfWidth,
                    halfHeight
                    ),
                capacity
                );
        }
    }
}

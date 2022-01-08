using Microsoft.Xna.Framework;
using System;

namespace TankEngine.DataStructures.Grid
{
    /// <summary>
    /// Class for creating a grid with difference purposes
    /// </summary>
    /// <typeparam name="T">The datatype of the grid</typeparam>
    public class Grid<T>
    {
        /// <summary>
        /// Internal data structure for the grid
        /// </summary>
        private readonly FlattenArray<T> gridData;

        /// <summary>
        /// The size of a single cell
        /// </summary>
        public float CellSize { get; }

        /// <summary>
        /// The start position for the grid
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// The width of the grid (Number of cells)
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The height of the grid (Number of cells)
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="width">The number of cells as width</param>
        /// <param name="height">The number of cells as height</param>
        /// <param name="cellSize">The size of a single cell</param>
        public Grid(int width, int height, float cellSize) : this(width, height, cellSize, Vector2.Zero) { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="width">The number of cells as width</param>
        /// <param name="height">The number of cells as height</param>
        /// <param name="cellSize">The size of a single cell</param>
        /// <param name="position">The position of the grid</param>
        public Grid(int width, int height, float cellSize, Vector2 position) : this(width, height, cellSize, Vector2.Zero, null) { }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="width">The number of cells as width</param>
        /// <param name="height">The number of cells as height</param>
        /// <param name="cellSize">The size of a single cell</param>
        /// <param name="position">The position of the grid</param>
        /// <param name="objectCreationMethod">Method to use for creating inital cell value, getting the x and y position of the cell as argument</param>
        public Grid(int width, int height, float cellSize, Vector2 position, Func<int, int, T> objectCreationMethod)
        {
            Width = width;
            Height = height;
            Position = position;
            CellSize = cellSize;
            gridData = new FlattenArray<T>(width, height);
            fillGrid(objectCreationMethod);
        }

        /// <summary>
        /// Fill the grid with inital data
        /// </summary>
        /// <param name="objectCreationMethod">The method used to create the inital cell content</param>
        private void fillGrid(Func<int, int, T> objectCreationMethod)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    T data = objectCreationMethod == null ? default(T) : objectCreationMethod(x, y);
                    gridData.SetValue(x, y, data);
                }
            }
        }

        /// <summary>
        /// Get the position in the cell for a given world position
        /// </summary>
        /// <param name="worldPosition">The position in the world</param>
        /// <returns>The point in the grid</returns>
        public Point GetPositionInGrid(Vector2 worldPosition)
        {
            Vector2 coordinatesInGrid = worldPosition - Position;
            int x = (int)Math.Floor(coordinatesInGrid.X / CellSize);
            int y = (int)Math.Floor(coordinatesInGrid.Y / CellSize);
            return new Point(x, y);
        }

        /// <summary>
        /// Get the world position for a cell in the grid
        /// </summary>
        /// <param name="x">The coordinate of the cell</param>
        /// <param name="y">The y coordinate of the cell</param>
        /// <returns>A vector 2 with the position of the cell in the world</returns>
        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(x, y) * CellSize * Position;
        }

        /// <summary>
        /// Get data of a cell
        /// </summary>
        /// <param name="worldPosition">The world position to get data for</param>
        /// <returns>The data of the cell from a given world position</returns>
        public T GetData(Vector2 worldPosition)
        {
            return GetData(GetPositionInGrid(worldPosition));
        }

        /// <summary>
        /// Get data of a cell
        /// </summary>
        /// <param name="gridPosition">The position in the grid to get data from (Grid coordinates)</param>
        /// <returns>The data of the cell</returns>
        public T GetData(Point gridPosition)
        {
            return GetData(gridPosition.X, gridPosition.Y);
        }

        /// <summary>
        /// Get data of a cell
        /// </summary>
        /// <param name="x">The x position in the grid to get data from (Grid coordinates)</param>
        /// <param name="y">The y position in the grid to get data from (Grid coordinates)</param>
        /// <returns>The data of the cell</returns>
        public T GetData(int x, int y)
        {
            return gridData.GetValue(x, y);
        }

        /// <summary>
        /// Set data of a cell
        /// </summary>
        /// <param name="worldPosition">The world position in the grid to set data to</param>
        /// <param name="value">The data to set in the cell</param>
        /// <returns>True if setting the data was successful</returns>
        public bool SetData(Vector2 worldPosition, T value)
        {
            return SetData(GetPositionInGrid(worldPosition), value);
        }

        /// <summary>
        /// Set data of a cell
        /// </summary>
        /// <param name="gridPosition">The position in the grid to set data to (Grid coordinates)</param>
        /// <param name="value">The data to set in the cell</param>
        /// <returns>True if setting the data was successful</returns>
        public bool SetData(Point gridPosition, T value)
        {
            return SetData(gridPosition.X, gridPosition.Y, value);
        }

        /// <summary>
        /// Set data of a cell
        /// </summary>
        /// <param name="x">The x position in the grid to set data to (Grid coordinates)</param>
        /// <param name="y">The y position in the grid to set data to (Grid coordinates)</param>
        /// <returns>True if setting the data was successful</returns>
        public bool SetData(int x, int y, T value)
        {
            return gridData.SetValue(x, y, value);
        }
    }
}

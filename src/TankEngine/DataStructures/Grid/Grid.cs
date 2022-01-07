using Microsoft.Xna.Framework;
using System;

namespace TankEngine.DataStructures.Grid
{
    public class Grid<T>
    {
        private readonly FlattenArray<T> gridData;

        public float CellSize { get; }
        public Vector2 Position { get; }

        public int Width { get; }
        public int Height { get; }

        public Grid(int width, int height, float cellSize) : this(width, height, cellSize, Vector2.Zero) { }

        public Grid(int width, int height, float cellSize, Vector2 position) : this(width, height, cellSize, Vector2.Zero, null) { }

        public Grid(int width, int height, float cellSize, Vector2 position, Func<int, int, T> objectCreationMethod)
        {
            Width = width;
            Height = height;
            Position = position;
            CellSize = cellSize;
            gridData = new FlattenArray<T>(width, height);
            fillGrid(objectCreationMethod);
        }

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

        public Point GetPositionInGrid(Vector2 worldPosition)
        {
            Vector2 coordinatesInGrid = worldPosition - Position;
            int x = (int)Math.Floor(coordinatesInGrid.X / CellSize);
            int y = (int)Math.Floor(coordinatesInGrid.Y / CellSize);
            return new Point(x, y);
        }

        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(x, y) * CellSize * Position;
        }

        public T GetData(Vector2 worldPosition)
        {
            return GetData(GetPositionInGrid(worldPosition));
        }
        public T GetData(Point gridPosition)
        {
            return GetData(gridPosition.X, gridPosition.Y);
        }
        public T GetData(int x, int y)
        {
            return gridData.GetValue(x, y);
        }

        public bool SetData(Vector2 worldPosition, T value)
        {
            return SetData(GetPositionInGrid(worldPosition), value);
        }
        public bool SetData(Point gridPosition, T value)
        {
            return SetData(gridPosition.X, gridPosition.Y, value);
        }
        public bool SetData(int x, int y, T value)
        {
            return gridData.SetValue(x, y, value);
        }
    }
}

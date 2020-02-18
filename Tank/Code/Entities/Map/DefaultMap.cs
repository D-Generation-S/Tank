using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.MapGenerators;
using Tank.src.DataStructure;

namespace Tank.Code.Entities.Map
{
    class DefaultMap : IMap
    {
        private Texture2D image;
        public Texture2D Image { get => image; }

        private FlattenArray<Color> imageData;

        private FlattenArray<Color> changedImageData;
        private FlattenArray<bool> changedCollisionMap;

        private FlattenArray<bool> collissionMap;
        public FlattenArray<bool> CollissionMap => collissionMap;

        private readonly int seed;
        public int Seed => seed;

        public int Height => image.Height;

        public int Width => image.Width;

        public DefaultMap(Texture2D image, FlattenArray<bool> collissionMap)
            : this(image, collissionMap, 0)
        {

        }

        public DefaultMap(Texture2D image, FlattenArray<bool> collissionMap, int seed)
        {
            this.image = image;
            Color[] tempData = new Color[image.Width * image.Height];

            image.GetData<Color>(tempData);
            imageData = new FlattenArray<Color>(tempData, image.Width);

            this.collissionMap = collissionMap;
            this.seed = seed;
        }

        public void SetPixel(Position position, Color color)
        {
            SetPixel(position.X, position.Y, color, true);
        }

        public void SetPixel(Position position, Color color, bool collidable)
        {
            SetPixel(position.X, position.Y, color, collidable);
        }

        public void SetPixel(int x, int y, Color color)
        {
            SetPixel(x, y, color, true);
        }

        public void SetPixel(int x, int y, Color color, bool collidable)
        {
            imageData.SetValue(x, y, color);
            collissionMap.SetValue(x, y, collidable);

            image.SetData(imageData.Array);
        }

        public void ChangePixel(Position position, Color color)
        {
            ChangePixel(position.X, position.Y, color, true);
        }

        public void ChangePixel(Position position, Color color, bool collidable)
        {
            ChangePixel(position.X, position.Y, color, collidable);
        }

        public void ChangePixel(int x, int y, Color color)
        {
            ChangePixel(x, y, color, true);
        }

        public void ChangePixel(int x, int y, Color color, bool collidable)
        {
            if (changedCollisionMap == null)
            {
                changedCollisionMap = new FlattenArray<bool>(collissionMap.Array, Width);
            }
                
            if (changedImageData == null)
            {
                changedImageData = new FlattenArray<Color>(imageData.Array, Width);
            }

            changedImageData.SetValue(x, y, color);
            changedCollisionMap.SetValue(x, y, collidable);
        }

        public void RevertChanges()
        {
            changedImageData = null;
            changedCollisionMap = null;
        }

        public void ApplyChanges()
        {
            imageData = changedImageData;
            image.SetData<Color>(imageData.Array);

            collissionMap = changedCollisionMap;

            RevertChanges();
        }

        public Color GetPixel(Position position)
        {
            return GetPixel(position.X, position.Y);
        }

        public Color GetPixel(int x, int y)
        {
            return imageData.GetValue(x, y);
        }

        public void RemovePixel(Position position)
        {
            RemovePixel(position.X, position.Y);
        }

        public void RemovePixel(int x, int y)
        {
            imageData.SetValue(x, y, Color.Transparent);
            collissionMap.SetValue(x, y, false);

            image.SetData(imageData.Array);
        }
    }
}

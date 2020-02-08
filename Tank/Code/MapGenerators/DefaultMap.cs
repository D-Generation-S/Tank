using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses;
using Tank.Code.DataContainer;
using Tank.Interfaces.MapGenerators;

namespace Tank.Code.MapGenerators
{
    class DefaultMap : BaseEntity, IMap
    {
        private Texture2D image;
        public Texture2D Image { get => image; }

        private FlattenArray<Color> imageData;

        private FlattenArray<bool> collissionMap;
        public FlattenArray<bool> CollissionMap => collissionMap;

        public DefaultMap(Texture2D image, FlattenArray<bool> collissionMap)
        {
            this.image = image;
            Color[] tempData = new Color[image.Width * image.Height];

            image.GetData<Color>(tempData);
            imageData = new FlattenArray<Color>(tempData, image.Width);

            this.collissionMap = collissionMap;
        }

        public void AddPixel(Position position, Color color)
        {
            AddPixel(position.X, position.Y, color, true);
        }

        public void AddPixel(Position position, Color color, bool collidable)
        {
            AddPixel(position.X, position.Y, color, collidable);
        }

        public void AddPixel(int x, int y, Color color)
        {
            AddPixel(x, y, color, true);
        }

        public void AddPixel(int x, int y, Color color, bool collidable)
        {
            imageData.SetValue(x, y, color);
            collissionMap.SetValue(x, y, collidable);

            image.SetData(imageData.Array);
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

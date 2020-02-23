using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.DataStructure;
using Tank.src.Interfaces.MapGenerators;

namespace Tank.Code.Entities.Map
{
    /// <summary>
    /// This class represents a default map
    /// </summary>
    class DefaultMap : IMap
    {
        /// <summary>
        /// The image to show for the map
        /// </summary>
        private Texture2D image;

        /// <summary>
        /// Readonly access to the image
        /// </summary>
        public Texture2D Image { get => image; }

        /// <summary>
        /// The image data for easier manipulation
        /// </summary>
        private FlattenArray<Color> imageData;

        /// <summary>
        /// The map to check collision on
        /// </summary>
        private FlattenArray<bool> collissionMap;

        /// <summary>
        /// Readonly access to the collision map
        /// </summary>
        public FlattenArray<bool> CollissionMap => collissionMap;

        /// <summary>
        /// A copy of the image data to make cached changes to apply later
        /// </summary>
        private FlattenArray<Color> changedImageData;

        /// <summary>
        /// The changed collision map chached for faster changing
        /// </summary>
        private FlattenArray<bool> changedCollisionMap;

        /// <summary>
        /// The seed the map was created with
        /// </summary>
        private readonly int seed;

        /// <summary>
        /// Readonly access to the seed the map was created with
        /// </summary>
        public int Seed => seed;

        /// <summary>
        /// Height of the map in pixels
        /// </summary>
        public int Height => image.Height;

        /// <summary>
        /// Width of the map in pixels
        /// </summary>
        public int Width => image.Width;

        /// <summary>
        /// Create a new instance for this class
        /// </summary>
        /// <param name="image">The image to use for this map</param>
        /// <param name="collissionMap">The collision map to use for the map</param>
        public DefaultMap(Texture2D image, FlattenArray<bool> collissionMap)
            : this(image, collissionMap, 0)
        {

        }

        /// <summary>
        /// Create a new instance for this class
        /// </summary>
        /// <param name="image">The image to use for this map</param>
        /// <param name="collissionMap">The collision map to use for the map</param>
        /// <param name="seed">The seed used to create the map</param>
        public DefaultMap(Texture2D image, FlattenArray<bool> collissionMap, int seed)
        {
            this.image = image;
            Color[] tempData = new Color[image.Width * image.Height];

            image.GetData<Color>(tempData);
            imageData = new FlattenArray<Color>(tempData, image.Width);

            this.collissionMap = collissionMap;
            this.seed = seed;
        }

        /// <summary>
        /// Set a pixel in the map to another Color and make it collideable
        /// </summary>
        /// <param name="position">Posiiotn of the pixel to set</param>
        /// <param name="color">The new color for the pixel</param>
        public void SetPixel(Position position, Color color)
        {
            SetPixel(position.X, position.Y, color, true);
        }


        /// <summary>
        /// Set a pixel in the map to another Color and make it collideable
        /// </summary>
        /// <param name="position">Posiiotn of the pixel to set the new color for</param>
        /// <param name="color">The new color for the pixel</param>
        /// <param name="collidable">Is this new pixel collideable</param>
        public void SetPixel(Position position, Color color, bool collidable)
        {
            SetPixel(position.X, position.Y, color, collidable);
        }

        /// <summary>
        /// Set a pixel in the map to another Color and make it collideable
        /// </summary>
        /// <param name="x">The x position of the pixel to change</param>
        /// <param name="y">The y position of the pixel to change</param>
        /// <param name="color">The new color for the pixel</param>
        public void SetPixel(int x, int y, Color color)
        {
            SetPixel(x, y, color, true);
        }

        /// <summary>
        /// Set a pixel in the map to another Color and make it collideable
        /// </summary>
        /// <param name="x">The x position of the pixel to change</param>
        /// <param name="y">The y position of the pixel to change</param>
        /// <param name="color">The new color for the pixel</param>
        /// <param name="collidable">Is this new pixel collideable</param>
        public void SetPixel(int x, int y, Color color, bool collidable)
        {
            imageData.SetValue(x, y, color);
            collissionMap.SetValue(x, y, collidable);

            image.SetData(imageData.Array);
        }

        /// <summary>
        /// Change a pixel in the cached storage
        /// </summary>
        /// <param name="position">Posiiotn of the pixel to set the new color for</param>
        /// <param name="color">The new color for the pixel</param>
        public void ChangePixel(Position position, Color color)
        {
            ChangePixel(position.X, position.Y, color, true);
        }

        /// <summary>
        /// Change a pixel in the cached storage
        /// </summary>
        /// <param name="position">Posiiotn of the cached pixel to set the new color for</param>
        /// <param name="color">The new color for the cached pixel</param>
        /// <param name="collidable">Is this cached pixel collideable</param>
        public void ChangePixel(Position position, Color color, bool collidable)
        {
            ChangePixel(position.X, position.Y, color, collidable);
        }

        /// <summary>
        /// Change a pixel in the cached storage
        /// </summary>
        /// <param name="x">The x position of the cached pixel to change</param>
        /// <param name="y">The y position of the cached pixel to change</param>
        /// <param name="color">The new color for the cached pixel</param>
        public void ChangePixel(int x, int y, Color color)
        {
            ChangePixel(x, y, color, true);
        }

        /// <summary>
        /// Change a pixel in the cached storage
        /// </summary>
        /// <param name="x">The x position of the cached pixel to change</param>
        /// <param name="y">The y position of the cached pixel to change</param>
        /// <param name="color">The new color for the cached pixel</param>
        /// <param name="collidable">Is this cached pixel collideable</param>
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

        /// <summary>
        /// Revert all the cached changed
        /// </summary>
        public void RevertChanges()
        {
            changedImageData = null;
            changedCollisionMap = null;
        }

        /// <summary>
        /// Apply the cached changes to the real map
        /// </summary>
        public void ApplyChanges()
        {
            imageData = changedImageData;
            image.SetData<Color>(imageData.Array);

            collissionMap = changedCollisionMap;

            RevertChanges();
        }

        /// <summary>
        /// Get the pixel on a specific position
        /// </summary>
        /// <param name="position">The position to get the pixel for</param>
        /// <returns>The color of the pixel at the given position</returns>
        public Color GetPixel(Position position)
        {
            return GetPixel(position.X, position.Y);
        }

        /// <summary>
        /// Get the pixel on a specific position
        /// </summary>
        /// <param name="x">The x position of the pixel to get</param>
        /// <param name="y">The y position of the pixel to get</param>
        /// <returns>The color of the pixel at the given position</returns>
        public Color GetPixel(int x, int y)
        {
            return imageData.GetValue(x, y);
        }

        /// <summary>
        /// Remove a pixel, making it transparent, at a given position
        /// </summary>
        /// <param name="position">The position of the pixel to remove</param>
        public void RemovePixel(Position position)
        {
            RemovePixel(position.X, position.Y);
        }

        /// <summary>
        /// Remove a pixel, making it transparent, at a given position
        /// </summary>
        /// <param name="x">The x position of the pixel to remove</param>
        /// <param name="y">The y position of the pixel to remove</param>
        public void RemovePixel(int x, int y)
        {
            imageData.SetValue(x, y, Color.Transparent);
            collissionMap.SetValue(x, y, false);

            image.SetData(imageData.Array);
        }
    }
}

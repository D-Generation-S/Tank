using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Tank.DataStructure.Spritesheet
{
    /// <summary>
    /// This class is representing a spritesheet it will provide you with some helper functions
    /// </summary>
    class SpriteSheet
    {
        /// <summary>
        /// An container containing the complete image
        /// </summary>
        private Texture2D completeImage;

        /// <summary>
        /// Readonly public access to the complete image
        /// </summary>
        public Texture2D CompleteImage => completeImage;

        /// <summary>
        /// The pixel data
        /// </summary>
        private readonly FlattenArray<Color> pixelData;

        /// <summary>
        /// The size of a single image in the container
        /// </summary>
        private Position singleImageSize;

        /// <summary>
        /// Readonly public access to the single image size
        /// </summary>
        public Position SingleImageSize => singleImageSize;

        /// <summary>
        /// The distance between images
        /// </summary>
        private int distanceBetweenImages;


        private List<SpriteSheetPattern> patterns;

        /// <summary>
        /// Readonly public access distance between images
        /// </summary>
        public int DistanceBetweenImages { get; }

        /// <summary>
        /// The maximal width and height to get images from
        /// </summary>
        private Position maxDimensions;

        /// <summary>
        /// Construct a new instance for this class
        /// </summary>
        /// <param name="image">The spritesheet itself</param>
        /// <param name="singleImageSize">The size of a single image in the spritesheet</param>
        /// <param name="distanceBetweenImages">The extra distrance between images</param>
        public SpriteSheet(Texture2D image, Position singleImageSize, int distanceBetweenImages)
            :this(image, singleImageSize, distanceBetweenImages, new List<SpriteSheetPattern>())
        {
        }

        /// <summary>
        /// Construct a new instance for this class
        /// </summary>
        /// <param name="image">The spritesheet itself</param>
        /// <param name="singleImageSize">The size of a single image in the spritesheet</param>
        /// <param name="distanceBetweenImages">The extra distrance between images</param>
        public SpriteSheet(Texture2D image, Position singleImageSize, int distanceBetweenImages, List<SpriteSheetPattern> patterns)
        {
            completeImage = image;
            this.singleImageSize = singleImageSize;
            this.distanceBetweenImages = distanceBetweenImages;
            SetSpriteSheetPattern(patterns);
            int xDimension = (int)Math.Floor((float)completeImage.Width / (singleImageSize.X + distanceBetweenImages));
            int yDimension = (int)Math.Floor((float)completeImage.Height / (singleImageSize.Y + distanceBetweenImages));
            maxDimensions = new Position(xDimension, yDimension);

            Color[] colors = new Color[completeImage.Width * completeImage.Height];
            image.GetData(colors);
            pixelData = new FlattenArray<Color>(colors, completeImage.Width);
        }

        public void SetSpriteSheetPattern(List<SpriteSheetPattern> patterns)
        {
            this.patterns = patterns;
        }

        public FlattenArray<Color> GetTextureByName(string name)
        {
            SpriteSheetPattern pattern = patterns.Find(pattern => pattern.Name == name);
            return GetColorFromPattern(pattern);
        }

        public FlattenArray<Color> GetTextureByPosition(int x, int y)
        {
            return null;
        }

        private FlattenArray<Color> GetColorFromPattern(SpriteSheetPattern pattern)
        {
            if (pattern == null)
            {
                return null;
            }

            Position startPosition = SingleImageSize * pattern.position;
            return GetColorFromSpriteAsFlatten(startPosition);
        }

        /// <summary>
        /// This will return you a special color area of your spritesheet
        /// </summary>
        /// <param name="startPosition">The start position of your area</param>
        /// <param name="endPosition">The end position or your area</param>
        /// <returns>An array with the colors in the given area</returns>
        public Color[] GetColorArea(Position startPosition, Position endPosition)
        {
            return GetColorArea(startPosition.X, startPosition.Y, endPosition.X, endPosition.Y);
        }

        /// <summary>
        /// This will return you a special color area of your spritesheet
        /// </summary>
        /// <param name="startX">The x start position of the area</param>
        /// <param name="startY">The y start position of the area</param>
        /// <param name="endX">The x end position of the area</param>
        /// <param name="endY">The y end position of the area</param>
        /// <returns></returns>
        public Color[] GetColorArea(int startX, int startY, int endX, int endY)
        {
            FlattenArray<Color> returnColor = new FlattenArray<Color>(endX - startX, endY - startY);
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    Color color = pixelData.GetValue(x, y);
                    returnColor.SetValue(x - startX, y - startY, color);
                }
            }

            return returnColor.Array;
        }

        /// <summary>
        /// Get the colors of a single picture of the spritesheet
        /// </summary>
        /// <param name="position">The x and y position of the sprite</param>
        /// <returns>The whole color of the requested sprite</returns>
        public Color[] GetColorFromSprite(Position position)
        {
            return GetColorFromSprite(position.X, position.Y);
        }

        /// <summary>
        /// Get the colors of a single picture of the spritesheet
        /// </summary>
        /// <param name="xPosition">The x position of the sprite</param>
        /// <param name="yPosition"></The y position of the sprite>
        /// <returns>The whole color of the requested sprite</returns>
        public Color[] GetColorFromSprite(int xPosition, int yPosition)
        {
            int xEnd = xPosition + singleImageSize.X;
            int yEnd = yPosition + singleImageSize.Y;
            return GetColorArea(xPosition, yPosition, xEnd, yEnd);
        }


        /// <summary>
        /// Get the colors of a single picture of the spritesheet
        /// </summary>
        /// <param name="xPosition">The x position of the sprite</param>
        /// <param name="yPosition"></The y position of the sprite>
        /// <returns>The whole color of the requested sprite</returns>
        public FlattenArray<Color> GetColorFromSpriteAsFlatten(Position position)
        {
            return GetColorFromSpriteAsFlatten(position.X, position.Y);
        }

        /// <summary>
        /// Get the colors of a single picture of the spritesheet
        /// </summary>
        /// <param name="xPosition">The x position of the sprite</param>
        /// <param name="yPosition"></The y position of the sprite>
        /// <returns>The whole color of the requested sprite</returns>
        public FlattenArray<Color> GetColorFromSpriteAsFlatten(int xPosition, int yPosition)
        {
            Color[] data = GetColorFromSprite(xPosition, yPosition);
            return new FlattenArray<Color>(data, singleImageSize.X);
        }
    }
}

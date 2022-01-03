using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Tank.DataStructure.Spritesheet
{
    /// <summary>
    /// This class is representing a spritesheet it will provide you with some helper functions
    /// </summary>
    public class SpriteSheet
    {
        /// <summary>
        /// Readonly public access to the complete image
        /// </summary>
        public Texture2D CompleteImage { get; }

        /// <summary>
        /// The pixel data
        /// </summary>
        private readonly FlattenArray<Color> pixelData;

        /// <summary>
        /// Readonly public access to the single image size
        /// </summary>
        public Point SingleImageSize { get; }

        /// <summary>
        /// The distance between images
        /// </summary>
        private int distanceBetweenImages;

        /// <summary>
        /// The patterns on this spritesheet
        /// </summary>
        private List<SpriteSheetPattern> patterns;

        /// <summary>
        /// Readonly public access distance between images
        /// </summary>
        public int DistanceBetweenImages { get; }

        /// <summary>
        /// The maximal width and height to get images from
        /// </summary>
        private Point maxDimensions;

        /// <summary>
        /// Construct a new instance for this class
        /// </summary>
        /// <param name="image">The spritesheet itself</param>
        /// <param name="singleImageSize">The size of a single image in the spritesheet</param>
        /// <param name="distanceBetweenImages">The extra distrance between images</param>
        public SpriteSheet(Texture2D image, Point singleImageSize, int distanceBetweenImages)
            : this(image, singleImageSize, distanceBetweenImages, new List<SpriteSheetPattern>())
        {
        }

        /// <summary>
        /// Construct a new instance for this class
        /// </summary>
        /// <param name="image">The spritesheet itself</param>
        /// <param name="singleImageSize">The size of a single image in the spritesheet</param>
        /// <param name="distanceBetweenImages">The extra distrance between images</param>
        public SpriteSheet(Texture2D image, Point singleImageSize, int distanceBetweenImages, List<SpriteSheetPattern> patterns)
        {
            CompleteImage = image;
            this.SingleImageSize = singleImageSize;
            this.distanceBetweenImages = distanceBetweenImages;
            SetSpriteSheetPattern(patterns);
            int xDimension = (int)Math.Floor((float)CompleteImage.Width / (singleImageSize.X + distanceBetweenImages));
            int yDimension = (int)Math.Floor((float)CompleteImage.Height / (singleImageSize.Y + distanceBetweenImages));
            maxDimensions = new Point(xDimension, yDimension);

            Color[] colors = new Color[CompleteImage.Width * CompleteImage.Height];
            image.GetData(colors);
            pixelData = new FlattenArray<Color>(colors, CompleteImage.Width);
        }

        /// <summary>
        /// Set the pattern for the spritesheet
        /// </summary>
        /// <param name="patterns">Tge pattern to use</param>
        public void SetSpriteSheetPattern(List<SpriteSheetPattern> patterns)
        {
            this.patterns = patterns;
        }

        /// <summary>
        /// Get a texture by name
        /// </summary>
        /// <param name="name">The name of the pattern</param>
        /// <returns>The texture image data</returns>
        public FlattenArray<Color> GetColorByName(string name)
        {
            SpriteSheetPattern pattern = patterns.Find(patter => patter.Name == name);
            return GetColorFromPattern(pattern);
        }

        /// <summary>
        /// Get a texture by position
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <returns></returns>
        public FlattenArray<Color> GetColorByPosition(int x, int y)
        {
            SpriteSheetPattern pattern = patterns.Find(patter => patter.Position.X == x && patter.Position.Y == y);
            return GetColorFromPattern(pattern);
        }

        /// <summary>
        /// Get a specific area defined by the pattern
        /// </summary>
        /// <param name="name">The name of the pattern</param>
        /// <returns>The usable rectangle</returns>
        public Rectangle GetAreaFromPattern(string name)
        {
            SpriteSheetPattern pattern = patterns.Find(patter => patter.Name == name);
            return GetAreaFromPattern(pattern);
        }

        /// <summary>
        /// Get a specific area defined by the pattern
        /// </summary>
        /// <param name="pattern">The pattern to get the data from</param>
        /// <returns>The usable rectangle</returns>
        public Rectangle GetAreaFromPattern(SpriteSheetPattern pattern)
        {
            if (pattern == null)
            {
                return Rectangle.Empty;
            }
            Point imageSize = pattern.SizeOverwritten ? pattern.PatternSizeOverwrite : SingleImageSize;
            int startPositionX = pattern.Position.X * imageSize.X;
            int startPositionY = pattern.Position.Y * imageSize.Y;

            return new Rectangle(startPositionX, startPositionY, imageSize.X, imageSize.Y);
        }

        /// <summary>
        /// Get the pattern image size
        /// </summary>
        /// <param name="name">The name of the pattern</param>
        /// <returns>The size of the image</returns>
        public Point GetPatternImageSize(string name)
        {
            SpriteSheetPattern pattern = patterns.Find(patter => patter.Name == name);
            return GetPatternImageSize(pattern);
        }

        /// <summary>
        /// Get the pattern image size
        /// </summary>
        /// <param name="pattern">The pattern to get the data from</param>
        /// <returns>The size of the image</returns>
        public Point GetPatternImageSize(SpriteSheetPattern pattern)
        {
            return pattern != null && pattern.SizeOverwritten ? pattern.PatternSizeOverwrite : SingleImageSize;
        }


        /// <summary>
        /// Get the color from the pattern
        /// </summary>
        /// <param name="pattern">The pattern to use</param>
        /// <returns>The color array</returns>
        private FlattenArray<Color> GetColorFromPattern(SpriteSheetPattern pattern)
        {
            if (pattern == null)
            {
                return null;
            }

            Point imageSize = pattern.SizeOverwritten ? pattern.PatternSizeOverwrite : SingleImageSize;
            Point startPosition = imageSize * pattern.Position;
            return GetColorFromSpriteAsFlatten(startPosition);
        }

        /// <summary>
        /// This will return you a special color area of your spritesheet
        /// </summary>
        /// <param name="startPosition">The start position of your area</param>
        /// <param name="endPosition">The end position or your area</param>
        /// <returns>An array with the colors in the given area</returns>
        public Color[] GetColorArea(Point startPosition, Point endPosition)
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
        public Color[] GetColorFromSprite(Point position)
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
            int xEnd = xPosition + SingleImageSize.X;
            int yEnd = yPosition + SingleImageSize.Y;
            return GetColorArea(xPosition, yPosition, xEnd, yEnd);
        }


        /// <summary>
        /// Get the colors of a single picture of the spritesheet
        /// </summary>
        /// <param name="xPosition">The x position of the sprite</param>
        /// <param name="yPosition"></The y position of the sprite>
        /// <returns>The whole color of the requested sprite</returns>
        public FlattenArray<Color> GetColorFromSpriteAsFlatten(Point position)
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
            return new FlattenArray<Color>(data, SingleImageSize.X);
        }
    }
}

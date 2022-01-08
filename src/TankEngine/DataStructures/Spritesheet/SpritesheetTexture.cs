using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TankEngine.DataStructures.Spritesheet
{
    /// <summary>
    /// Wrapper class for spritesheet data extendet with texture information
    /// </summary>
    public class SpritesheetTexture : ISpritesheetData
    {
        /// <summary>
        /// The spritesheet data for this texture
        /// </summary>
        private readonly ISpritesheetData spritesheetData;

        /// <summary>
        /// The texture the spritesheet data belongs to
        /// </summary>
        public Texture2D Texture { get; }

        /// <summary>
        /// The color's from the texture
        /// </summary>
        private FlattenArray<Color> imageColors;

        public bool Ready => Texture != null && spritesheetData != null;

        /// <inheritdoc/>
        public string ImageName => spritesheetData?.ImageName;

        /// <inheritdoc/>
        public string ImageNameWithoutExtension => spritesheetData?.ImageNameWithoutExtension;

        /// <inheritdoc/>
        public Point ImageSize => spritesheetData == null ? Point.Zero : spritesheetData.ImageSize;

        /// <inheritdoc/>
        public float ImageScale => spritesheetData == null ? 0 : spritesheetData.ImageScale;

        /// <inheritdoc/>
        public List<SpritesheetArea> Areas => spritesheetData?.Areas;

        /// <inheritdoc/>
        public List<SpritesheetFrame> Frames => spritesheetData?.Frames;

        /// <inheritdoc/>
        public List<SpritesheetFrameTag> FrameTags => spritesheetData?.FrameTags;

        /// <summary>
        /// Create a new spritesheet texture
        /// </summary>
        /// <param name="spritesheetData">The spritesheet data to use</param>
        /// <param name="texture">The texture loaded from the spritesheet</param>
        public SpritesheetTexture(ISpritesheetData spritesheetData, Texture2D texture)
        {
            this.spritesheetData = spritesheetData;
            Texture = texture;
        }

        /// <summary>
        /// Create a new spritesheet texture
        /// </summary>
        /// <param name="spritesheetData">The spritesheet data to use</param>
        /// <param name="loadTextureMethod">Method to load the texture</param>
        /// <exception cref="ArgumentNullException">Exception if load method is not given</exception>
        public SpritesheetTexture(ISpritesheetData spritesheetData, Func<ISpritesheetData, Texture2D> loadTextureMethod)
        {
            if (loadTextureMethod == null)
            {
                throw new ArgumentNullException(nameof(loadTextureMethod));
            }
            this.spritesheetData = spritesheetData;
            Texture = loadTextureMethod(spritesheetData);
        }

        /// <inheritdoc/>
        public SpritesheetFrameTag GetTagByName(string name)
        {
            return spritesheetData.GetTagByName(name);
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetTagNames()
        {
            return spritesheetData?.GetTagNames();
        }

        /// <inheritdoc/>
        public IEnumerable<SpritesheetFrame> GetFrames(SpritesheetFrameTag tag)
        {
            return spritesheetData?.GetFrames(tag);
        }

        /// <summary>
        /// Method to load the color map once to make the data available
        /// </summary>
        public void PreloadColorMap()
        {
            GetImageColors();
        }

        /// <summary>
        /// Get the color of the image
        /// </summary>
        /// <returns>The color as a flatten array</returns>
        public FlattenArray<Color> GetImageColors()
        {
            if (imageColors == null)
            {
                Color[] colors = new Color[Texture.Width * Texture.Height];
                Texture.GetData(colors);
                imageColors = new FlattenArray<Color>(colors, Texture.Width);
            }
            return imageColors;
        }

        /// <summary>
        /// Get the color from a part of the image
        /// </summary>
        /// <param name="area">The area to get the data for</param>
        /// <returns>The part of the texture or null if values are not on the texture grid</returns>
        public FlattenArray<Color> GetColorFromArea(SpritesheetArea area)
        {
            return GetColorFromArea(area.Area);
        }

        /// <summary>
        /// Get the color from a part of the image
        /// </summary>
        /// <param name="rectangle">The are to get the data for</param>
        /// <returns>The part of the texture or null if values are not on the texture grid</returns>
        public FlattenArray<Color> GetColorFromArea(Rectangle rectangle)
        {
            return GetImageColors()?.GetArea(rectangle);
        }
    }
}

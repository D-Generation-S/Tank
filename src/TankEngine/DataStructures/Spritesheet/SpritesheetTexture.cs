using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TankEngine.DataStructures.Spritesheet
{
    public class SpritesheetTexture : ISpritesheetData
    {
        private readonly ISpritesheetData spritesheetData;

        public Texture2D Texture { get; }

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
    }
}

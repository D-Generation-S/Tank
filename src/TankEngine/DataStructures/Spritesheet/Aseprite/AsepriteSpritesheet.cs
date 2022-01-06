using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TankEngine.DataStructures.Spritesheet.Aseprite
{
    /// <summary>
    /// Spritesheet for aseprite data files
    /// </summary>
    public class AsepriteSpritesheet : ISpritesheet
    {
        /// <inheritdoc/>
        public string ImageName { get; }

        /// <inheritdoc/>
        public Point ImageSize { get; }

        /// <inheritdoc/>
        public float ImageScale { get; }

        /// <inheritdoc/>
        public List<SpritesheetArea> Areas { get; }

        /// <inheritdoc/>
        public List<SpritesheetFrame> Frames { get; }

        /// <inheritdoc/>
        public List<SpritesheetFrameTag> FrameTags { get; }

        /// <summary>
        /// Create a new instance of this class form a aseprite file data set
        /// </summary>
        /// <param name="asepriteArrayFileData">The aseprite file data set to use as a base</param>
        public AsepriteSpritesheet(AsepriteArrayFileData asepriteArrayFileData)
        {
            ImageName = asepriteArrayFileData.Meta.Image;
            ImageSize = asepriteArrayFileData.Meta.Size.GetPoint();
            ImageScale = 1;
            try
            {
                float parsedScale;
                float.TryParse(asepriteArrayFileData.Meta.Scale, out parsedScale);
                ImageScale = parsedScale;
            }
            catch (Exception)
            {
            }
            Areas = asepriteArrayFileData.Meta.Slices.SelectMany(slice => slice.GetSpritesheetArea()).ToList();
            Frames = asepriteArrayFileData.Frames.Select(frame => frame.GetSpritesheetFrame()).ToList();
            FrameTags = asepriteArrayFileData.Meta.FrameTags.Select(tag => tag.GetSpritesheetFrameTag()).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<SpritesheetFrame> GetFrames(SpritesheetFrameTag tag)
        {
            int frameNumberToTake = (tag.EndFrame - tag.StartFrame) + 1;
            return Frames.Skip(tag.StartFrame).Take(frameNumberToTake);
        }

        /// <inheritdoc/>
        public SpritesheetFrameTag GetTagByName(string name)
        {
            return FrameTags.FirstOrDefault(tag => tag.Name == name);
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetTagNames()
        {
            return FrameTags.Select(tag => tag.Name);
        }
    }
}

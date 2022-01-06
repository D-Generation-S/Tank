using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TankEngine.DataStructures.Spritesheet
{
    /// <summary>
    /// Spritesheet data for a single spritesheet file
    /// </summary>
    public interface ISpritesheet
    {
        /// <summary>
        /// The name of the image this dataset belongs to
        /// </summary>
        string ImageName { get; }

        /// <summary>
        /// The size of the image for this data set
        /// </summary>
        Point ImageSize { get; }

        /// <summary>
        /// The scaling for the image of this data set
        /// </summary>
        float ImageScale { get; }

        /// <summary>
        /// All the tagged areas in the image
        /// </summary>
        List<SpritesheetArea> Areas { get; }

        /// <summary>
        /// all the single frames in the image for animation purpose
        /// </summary>
        List<SpritesheetFrame> Frames { get; }

        /// <summary>
        /// The frame tags of the spritesheet
        /// </summary>
        List<SpritesheetFrameTag> FrameTags { get; }

        /// <summary>
        /// Get a single tag by it's name
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>The tag with the given name or null</returns>
        SpritesheetFrameTag GetTagByName(string name);

        /// <summary>
        /// Get all the tag names
        /// </summary>
        /// <returns>All the tag names for this spritesheet</returns>
        IEnumerable<string> GetTagNames();

        /// <summary>
        /// Get all the frames for a single tag
        /// </summary>
        /// <param name="tag">The tag to get the frames for</param>
        /// <returns>A list with all the frames</returns>
        IEnumerable<SpritesheetFrame> GetFrames(SpritesheetFrameTag tag);
    }
}

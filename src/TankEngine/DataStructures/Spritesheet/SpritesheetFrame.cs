using Microsoft.Xna.Framework;
using TankEngine.DataStructures.Serializeable;

namespace TankEngine.DataStructures.Spritesheet
{
    /// <summary>
    /// Class to describe a single spritesheet frame
    /// </summary>
    public class SpritesheetFrame
    {
        /// <summary>
        /// The area of the frame on the spritesheet
        /// </summary>
        public Rectangle Frame { get; }

        /// <summary>
        /// The size of the source frame
        /// </summary>
        public Rectangle SpriteSize { get; }

        public Point SourceSize { get; }

        /// <summary>
        /// How long the frame should be shown in milliseconds
        /// </summary>
        public int Duration { get; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="frame">The area for the frame</param>
        /// <param name="spriteSize">The size of the source frame</param>
        /// <param name="sourceSize"></param>
        /// <param name="duration">The duration of the frame in milliseconds</param>
        public SpritesheetFrame(Rectangle frame, Rectangle spriteSize, Point sourceSize, int duration)
        {
            Frame = frame;
            SpriteSize = spriteSize;
            SourceSize = sourceSize;
            Duration = duration;
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="frame">The area for the frame</param>
        /// <param name="spriteSize">The size of the source frame</param>
        /// <param name="sourceSize"></param>
        /// <param name="duration">The duration of the frame in milliseconds</param>
        public SpritesheetFrame(SRectangle frame, SRectangle spriteSize, SDimension sourceSize, int duration)
            : this(frame.GetRectangle(), spriteSize.GetRectangle(), sourceSize.GetPoint(), duration) { }
    }

}

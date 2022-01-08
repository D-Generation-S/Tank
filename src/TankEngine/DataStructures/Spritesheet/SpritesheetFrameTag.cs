namespace TankEngine.DataStructures.Spritesheet
{
    /// <summary>
    /// A tagged part of a frame animation with a name and definition at which index it starts and ends
    /// </summary>
    public class SpritesheetFrameTag
    {
        /// <summary>
        /// The name of the collection of frames
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The index where the frames are starting
        /// </summary>
        public int StartFrame { get; }

        /// <summary>
        /// The index where the frame are ending
        /// </summary>
        public int EndFrame { get; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="name">The name of the frame collection</param>
        /// <param name="startFrame">The index where the collection starts on the frame collection</param>
        /// <param name="endFrame">The index where the collection ends on the frame collection</param>
        public SpritesheetFrameTag(string name, int startFrame, int endFrame)
        {
            Name = name;
            StartFrame = startFrame;
            EndFrame = endFrame;
        }
    }
}

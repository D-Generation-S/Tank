namespace TankEngine.DataStructures.Spritesheet
{
    public class SpritesheetFrameTag
    {
        public string Name { get; }

        public int StartFrame { get; }

        public int EndFrame { get; }

        public SpritesheetFrameTag(string name, int startFrame, int endFrame)
        {
            Name = name;
            StartFrame = startFrame;
            EndFrame = endFrame;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TankEngine.DataStructures.Spritesheet.Aseprite
{
    public class AsepriteSpritesheet : ISpritesheet
    {
        public string ImageName { get; }

        public Point ImageSize { get; }

        public float ImageScale { get; }

        public List<SpritesheetArea> Areas { get; }

        public List<SpritesheetFrame> Frames { get; }

        public List<SpritesheetFrameTag> FrameTags { get; }

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

        public IEnumerable<SpritesheetFrame> GetFrames(SpritesheetFrameTag tag)
        {
            return Frames.Skip(tag.StartFrame).Take(tag.EndFrame - tag.StartFrame);
        }

        public SpritesheetFrameTag GetTagByName(string name)
        {
            return FrameTags.FirstOrDefault(tag => tag.Name == name);
        }

        public IEnumerable<string> GetTagNames()
        {
            return FrameTags.Select(tag => tag.Name);
        }
    }
}

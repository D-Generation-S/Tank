using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tank.Code.SubClasses
{
    public class ColorTextureAssignment
    {
        public Color ColorToReplace
        {
            get; set;
        }
        public int Width
        {
            get; set;
        }
        public int Height
        {
            get; set;
        }
        public Color[] ColorData
        {
            get; set;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Components.Rendering
{
    class VisibleTextComponent : BaseVisibleComponent
    {
        public string Text { get; set; }

        public SpriteFont Font { get; set; }

        public float Scale { get; set; }

        public VisibleTextComponent()
        {
            Scale = 1f;
        }
    }
}

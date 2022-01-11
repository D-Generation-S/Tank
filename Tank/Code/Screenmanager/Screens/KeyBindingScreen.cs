using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Tank.Enums;

namespace Tank.Code.Screenmanager
{
    class KeyBindingScreen : BasicScreen
    {
        public KeyBindingScreen(ScreenType screentype, int ScreenWidth, int ScreenHeigh, GraphicsDevice GD) : base(screentype, ScreenWidth, ScreenHeigh, GD)
        {
            Name = "KeyBindingScreen";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Tank.Enums;

namespace Tank.Code.Screenmanager
{
    class BasicBackgroundScreen : BasicScreen
    {
        internal BasicScreen _backgroundScreen;
        

        public BasicBackgroundScreen(ScreenType screentype, int ScreenWidth, int ScreenHeigh, GraphicsDevice GD, BasicScreen BackgroundScreen = null) : base(screentype, ScreenWidth, ScreenHeigh, GD)
        {
            _backgroundScreen = BackgroundScreen;
            if (_backgroundScreen != null)
                _backgroundScreen.IsBackgroundScreen = true;
            
        }

        public override void Draw(SpriteBatch SB, GraphicsDevice GD)
        {
            if (_backgroundScreen != null)
                _backgroundScreen.Draw(SB, GD);
            base.Draw(SB, GD);
        }

        public override void DisableScreen()
        {
            if (_backgroundScreen != null)
                _backgroundScreen.DisableScreen();
            Renderer.Instance.OverlayerClear();
            base.DisableScreen();
            if (!Delete)
                _backgroundScreen.ActivateScreen();
        }
    }
}

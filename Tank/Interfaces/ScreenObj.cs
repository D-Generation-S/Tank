using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tank.Interfaces
{
    public interface IScreenObj
    {
        void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameActive);

        void Draw(SpriteBatch SB, GraphicsDevice GD);

        void ActivateScreen(bool FirstScreen);

        void DisableScreen();
    }
}

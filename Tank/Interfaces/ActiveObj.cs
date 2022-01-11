using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tank.Interfaces
{
    public interface IActiveObj
    {
        bool Active
        {
            get;
            set;
        }
        void Update(MouseState mouseState, KeyboardState keyboardState, GameTime currentGameTime, GamePadState gsState);
    }
}

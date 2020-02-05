using Microsoft.Xna.Framework.Input;

namespace Tank.Code
{
    public static class Controls
    {
        public static bool KeyToogle(Keys PressedKey)
        {
            bool bReturn = Settings.CurrentKeyboardState.IsKeyDown(PressedKey) && !Settings.PreviousKeyboardState.IsKeyDown(PressedKey);
            return bReturn;
        }


        public static void Update()
        {

        }

        public static bool KeyPressed(this KeyboardState State, Keys[] KeyArray)
        {
            foreach (Keys key in KeyArray)
            {
                if (State.IsKeyDown(key))
                    return true;
            }
            return false;
        }
    }
}

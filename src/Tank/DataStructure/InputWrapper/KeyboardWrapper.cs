using Microsoft.Xna.Framework.Input;

namespace Tank.DataStructure.InputWrapper
{
    class KeyboardWrapper
    {
        private KeyboardState previousKeyState;
        private KeyboardState currentKeyState;

        public void UpdateState()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
        }

        public KeyboardState GetState()
        {
            UpdateState();
            return currentKeyState;
        }

        public KeyboardState GetPreviousState()
        {
            return previousKeyState;
        }

        public bool IsKeyDown(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return currentKeyState.IsKeyUp(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key);
        }
    }
}

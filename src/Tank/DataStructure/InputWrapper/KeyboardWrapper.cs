using Microsoft.Xna.Framework.Input;

namespace Tank.DataStructure.InputWrapper
{
    /// <summary>
    /// This class will wrap the keyboard
    /// </summary>
    class KeyboardWrapper
    {
        /// <summary>
        /// Previous keyboard state
        /// </summary>
        private KeyboardState previousKeyState;

        /// <summary>
        /// Current keyboard stae
        /// </summary>
        private KeyboardState currentKeyState;

        /// <summary>
        /// Update the state
        /// </summary>
        public void UpdateState()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
        }

        /// <summary>
        /// Get the state
        /// </summary>
        /// <returns>The current state</returns>
        public KeyboardState GetState()
        {
            UpdateState();
            return currentKeyState;
        }

        /// <summary>
        /// Get the prevoius state
        /// </summary>
        /// <returns>The previus state</returns>
        public KeyboardState GetPreviousState()
        {
            return previousKeyState;
        }

        /// <summary>
        /// Is a specific key down
        /// </summary>
        /// <param name="key">The key which is down</param>
        /// <returns>True if key is down</returns>
        public bool IsKeyDown(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        /// <summary>
        /// Is a specific key up
        /// </summary>
        /// <param name="key">The key which is up</param>
        /// <returns>True if key is up</returns>
        public bool IsKeyUp(Keys key)
        {
            return currentKeyState.IsKeyUp(key);
        }

        /// <summary>
        /// Is a specific key pressed
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>True if the key is currently pressed</returns>
        public bool IsKeyPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key);
        }
    }
}

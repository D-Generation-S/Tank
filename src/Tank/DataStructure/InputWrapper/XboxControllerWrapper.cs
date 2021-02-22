using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tank.DataStructure.InputWrapper
{
    /// <summary>
    /// Class to wrap the xbox controller
    /// </summary>
    class XboxControllerWrapper
    {
        /// <summary>
        /// The player index for the controller
        /// </summary>
        private readonly PlayerIndex index;

        /// <summary>
        /// The current state of the pad
        /// </summary>
        private GamePadState currentPadState;

        /// <summary>
        /// Create a new instance of this wrapper
        /// </summary>
        /// <param name="index">The player index</param>
        public XboxControllerWrapper(PlayerIndex index)
        {
            this.index = new PlayerIndex();
        }

        /// <summary>
        /// Update the state
        /// </summary>
        public void UpdateState()
        {
            //previousKeyState = currentKeyState;
            currentPadState = GamePad.GetState(index);
        }

        /// <summary>
        /// Is a specific key down
        /// </summary>
        /// <returns>True is key is down</returns>
        public bool IsKeyDown()
        {
            return currentPadState.Triggers.Right > 0;
        }
    }
}

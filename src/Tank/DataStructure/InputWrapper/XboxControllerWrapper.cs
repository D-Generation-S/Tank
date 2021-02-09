using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tank.src.DataStructure.InputWrapper
{
    class XboxControllerWrapper
    {
        private readonly PlayerIndex index;

        private GamePadState currentPadState;

        public XboxControllerWrapper(PlayerIndex index)
        {
            this.index = new PlayerIndex();
        }

        public void UpdateState()
        {
            //previousKeyState = currentKeyState;
            currentPadState = GamePad.GetState(index);
        }

        public bool IsKeyDown()
        {
            return currentPadState.Triggers.Right > 0;
        }
    }
}

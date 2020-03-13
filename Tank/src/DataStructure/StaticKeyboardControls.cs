using Microsoft.Xna.Framework.Input;
using Tank.src.DataStructure.InputWrapper;
using Tank.src.Interfaces.GameObjectControlling;

namespace Tank.src.DataStructure
{
    class StaticKeyboardControls : IGameObjectController
    {
        /// <summary>
        /// The keyboard wrapper to use
        /// </summary>
        private readonly KeyboardWrapper keyboardWrapper;

        private Keys fire = Keys.Space;

        private Keys strenghtUp = Keys.W;

        private Keys strenghtDown = Keys.S;

        private Keys rotationUp = Keys.A;

        private Keys rotationDown = Keys.D;

        /// <summary>
        /// Create a new instance of this static keyboard control
        /// </summary>
        public StaticKeyboardControls()
        {
            keyboardWrapper = new KeyboardWrapper();
        }

        /// <inheritdoc/>
        public void UpdateStates()
        {
            keyboardWrapper.UpdateState();
        }

        /// <inheritdoc/>
        public bool IsFirePressed()
        {
            return keyboardWrapper.IsKeyPressed(fire);
        }

        /// <inheritdoc/>
        public bool IncreaseStrength()
        {
            return keyboardWrapper.IsKeyDown(strenghtDown);
        }

        /// <inheritdoc/>
        public bool DecreseStrength()
        {
            return keyboardWrapper.IsKeyDown(strenghtUp);
        }

        /// <inheritdoc/>
        public bool RotateUp()
        {
            return keyboardWrapper.IsKeyDown(rotationUp);
        }

        /// <inheritdoc/>
        public bool RotateDown()
        {
            return keyboardWrapper.IsKeyDown(rotationDown);
        }
    }
}

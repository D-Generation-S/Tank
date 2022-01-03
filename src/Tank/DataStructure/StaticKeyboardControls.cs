using Microsoft.Xna.Framework.Input;
using Tank.DataStructure.InputWrapper;
using Tank.Interfaces.GameObjectControlling;

namespace Tank.DataStructure
{
    public class StaticKeyboardControls : IGameObjectController
    {
        /// <summary>
        /// The keyboard wrapper to use
        /// </summary>
        private readonly KeyboardWrapper keyboardWrapper;

        /// <summary>
        /// The fire key to use
        /// </summary>
        private Keys fire = Keys.Space;

        /// <summary>
        /// Increase the strenght
        /// </summary>
        private Keys strenghtUp = Keys.W;

        /// <summary>
        /// Decrease the strengh
        /// </summary>
        private Keys strenghtDown = Keys.S;

        /// <summary>
        /// Rotate the barrel up
        /// </summary>
        private Keys rotationUp = Keys.A;

        /// <summary>
        /// Rotate the barrel down
        /// </summary>
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

using System;
using Tank.DataStructure.InputWrapper;
using Tank.Interfaces.GameObjectControlling;

namespace Tank.DataStructure
{
    public class StaticXboxController : IGameObjectController
    {
        /// <summary>
        /// The warpper to use
        /// </summary>
        private readonly XboxControllerWrapper xboxControllerWrapper;

        /// <summary>
        /// Create a new instance of this
        /// </summary>
        public StaticXboxController()
        {
            xboxControllerWrapper = new XboxControllerWrapper(Microsoft.Xna.Framework.PlayerIndex.One);
        }

        /// <inheritdoc/>
        public bool DecreseStrength()
        {
            return xboxControllerWrapper.IsKeyDown();
        }

        /// <inheritdoc/>
        public bool IncreaseStrength()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool IsFirePressed()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool RotateDown()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool RotateUp()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void UpdateStates()
        {
            xboxControllerWrapper.UpdateState();
        }
    }
}

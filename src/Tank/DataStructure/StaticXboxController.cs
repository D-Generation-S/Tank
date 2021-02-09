using System;
using Tank.src.DataStructure.InputWrapper;
using Tank.src.Interfaces.GameObjectControlling;

namespace Tank.src.DataStructure
{
    class StaticXboxController : IGameObjectController
    {
        private readonly XboxControllerWrapper xboxControllerWrapper;

        public StaticXboxController()
        {
            xboxControllerWrapper = new XboxControllerWrapper(Microsoft.Xna.Framework.PlayerIndex.One);
        }

        public bool DecreseStrength()
        {
            return xboxControllerWrapper.IsKeyDown();
        }

        public bool IncreaseStrength()
        {
            throw new NotImplementedException();
        }

        public bool IsFirePressed()
        {
            throw new NotImplementedException();
        }

        public bool RotateDown()
        {
            throw new NotImplementedException();
        }

        public bool RotateUp()
        {
            throw new NotImplementedException();
        }

        public void UpdateStates()
        {
            xboxControllerWrapper.UpdateState();
        }
    }
}

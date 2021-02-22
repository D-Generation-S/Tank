using System;
using Tank.DataStructure.InputWrapper;
using Tank.Interfaces.GameObjectControlling;

namespace Tank.DataStructure
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

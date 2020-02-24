using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.GameObjectControlling;

namespace Tank.src.DataStructure
{
    class StaticKeyboardControls : IGameObjectController
    {
        private Keys fire = Keys.Space;
        public Keys Fire => fire;

        private Keys strenghtUp = Keys.W;
        public Keys StrenghtUp => strenghtUp;

        private Keys strenghtDown = Keys.S;
        public Keys StrenghtDown => strenghtDown;

        private Keys rotationUp = Keys.A;
        public Keys RotationUp => rotationUp;

        private Keys rotationDown = Keys.D;
        public Keys RotationDown => rotationDown;
    }
}

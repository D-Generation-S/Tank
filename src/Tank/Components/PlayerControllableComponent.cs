using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.GameObjectControlling;

namespace Tank.Components
{
    /// <summary>
    /// This object is controlable by a player
    /// </summary>
    class PlayerControllableComponent : ControllableGameObject
    {
        private readonly IGameObjectController controller;
        public IGameObjectController Controller => controller;

        public PlayerControllableComponent(IGameObjectController objectController)
        {
            controller = objectController;
        }
    }
}

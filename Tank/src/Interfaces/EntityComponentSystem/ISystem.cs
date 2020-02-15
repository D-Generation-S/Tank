using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.src.Interfaces.EntityComponentSystem
{
    interface ISystem : IEventReceiver
    {
        /// <summary>
        /// Initialize this system
        /// </summary>
        /// <param name="gameEngine">The current game engine</param>
        void Initialize(IGameEngine gameEngine);

        /// <summary>
        /// Update this system
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draw this system
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        void Draw(GameTime gameTime);
    }
}

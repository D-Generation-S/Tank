using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Wrapper;

namespace Tank.src.Interfaces.EntityComponentSystem.Manager
{
    interface IGameEngine
    {
        IEventManager EventManager { get; }

        IEntityManager EntityManager { get; }

        ContentWrapper ContentManager { get;  }

        /// <summary>
        /// This method allows you to add a new system
        /// </summary>
        /// <param name="systemToAdd">The current system to add</param>
        void AddSystem(ISystem systemToAdd);

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

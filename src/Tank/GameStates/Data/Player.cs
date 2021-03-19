using System;
using System.Collections.Generic;
using System.Text;
using Tank.Interfaces.Builders;

namespace Tank.GameStates.Data
{
    class Player
    {
        /// <summary>
        /// The player name
        /// </summary>
        public string PlayerName { get; }

        /// <summary>
        /// The tank builder to use
        /// </summary>
        public IGameObjectBuilder TankBuilder { get; }

        public Player(string playerName, IGameObjectBuilder tankBuilder)
        {
            PlayerName = playerName;
            TankBuilder = tankBuilder;
        }
    }
}

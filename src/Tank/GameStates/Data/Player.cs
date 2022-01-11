using Tank.Enums;
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
        /// The player number
        /// </summary>
        public int PlayerNumber { get; }

        /// <summary>
        /// The team of the player
        /// </summary>
        public int Team { get; }

        /// <summary>
        /// The control type to use
        /// </summary>
        public ControlTypeEnum ControlType { get; }

        /// <summary>
        /// The tank builder to use
        /// </summary>
        public IGameObjectBuilder TankBuilder { get; }

        /// <summary>
        /// Is this player an ai player
        /// </summary>
        public bool IsAi => PlayerType != PlayerTypeEnum.Player;

        /// <summary>
        /// Type of this player
        /// </summary>
        public PlayerTypeEnum PlayerType;

        public Player(string playerName, int playerNumber, ControlTypeEnum controlType, int team, PlayerTypeEnum playerType, IGameObjectBuilder tankBuilder)
        {
            PlayerName = playerName;
            PlayerNumber = playerNumber;
            Team = team;
            ControlType = controlType;
            TankBuilder = tankBuilder;

            PlayerType = playerType;
        }
    }
}

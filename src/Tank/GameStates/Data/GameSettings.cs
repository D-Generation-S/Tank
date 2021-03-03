namespace Tank.GameStates.Data
{
    /// <summary>
    /// Class to contain the current game settings
    /// </summary>
    class GameSettings
    {
        /// <summary>
        /// The number of players
        /// </summary>
        public uint PlayerCount { get; }

        /// <summary>
        /// The level of gravity
        /// </summary>
        public float Gravity { get; }

        /// <summary>
        /// The wind to apply
        /// </summary>
        public float Wind { get; }

        /// <summary>
        /// The name of the spriteset to load for texturizing
        /// </summary>
        public string SpriteSetName { get; }

        /// <summary>
        /// The map seed to use for generating
        /// </summary>
        public int MapSeed { get; }

        /// <summary>
        /// Is the game in debug
        /// </summary>
        public bool IsDebug { get; private set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="gravity">How strong the gravity is</param>
        /// <param name="wind">How strong the wind should be</param>
        /// <param name="playerCount">The number of players</param>
        /// <param name="mapSeed">The map seed to use</param>
        /// <param name="spriteSetName">The name of the spriteset to load</param>
        public GameSettings(float gravity, float wind, uint playerCount, int mapSeed, string spriteSetName)
        {
            Gravity = gravity;
            Wind = wind;
            PlayerCount = playerCount;
            MapSeed = mapSeed;
            SpriteSetName = spriteSetName;
            IsDebug = false;
        }

        /// <summary>
        /// Make the game debug
        /// </summary>
        public void SetDebug()
        {
            IsDebug = true;
        }
    }
}

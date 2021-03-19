using Microsoft.Xna.Framework;

namespace Tank.GameStates.Data
{
    class PlayerStartData
    {
        public Vector2 StartPosition { get; }
        public string PlayerName { get; }

        public PlayerStartData(string playerName, Vector2 startPosition)
        {
            PlayerName = playerName;
            StartPosition = startPosition;
        }
    }
}

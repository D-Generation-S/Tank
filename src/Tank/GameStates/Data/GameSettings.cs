using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.GameStates.Data
{
    class GameSettings
    {
        public uint PlayerCount { get; }
        public float Gravity { get; }
        public float Wind { get; }
        public string SpriteSetName { get; }
        public bool IsDebug { get; private set; }

        public GameSettings(float gravity, float wind, uint playerCount, string spriteSetName)
        {
            Gravity = gravity;
            Wind = wind;
            PlayerCount = playerCount;
            SpriteSetName = spriteSetName;
            IsDebug = false;
        }

        public void SetDebug()
        {
            IsDebug = true;
        }
    }
}

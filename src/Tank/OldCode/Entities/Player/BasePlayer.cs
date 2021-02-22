using Microsoft.Xna.Framework;
using System;
using Tank.Code.GUIClasses;

namespace Tank.Code.Entities.Player
{
    public class BasePlayer
    {
        protected readonly float graphicUpdateTime = 50;
        protected float lastGraphicUpdateTime = 0;
        protected GameTime previousGameTime;
        protected Animation activityIndicator;

        public PlayerIndex Index
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public int Money
        {
            get; set;
        }
        public Vehicle Tank
        {
            get; set;
        }
        public PlayerOverlay Overlay
        {
            get; set;
        }
        public bool IsActive
        {
            get; set;
        }

        public event EventHandler OnFinish;
        public event EventHandler OnDeath;

        public BasePlayer(PlayerIndex index)
        {
            Index = index;
            Money = 1000000;
        }
    }
}
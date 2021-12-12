using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Components.GameObject
{
    class PlayerStatisticComponent : BaseComponent
    {
        public string Name;
        public int Team;
        public int Points;
        public int Kills;

        public override void Init()
        {
            Name = string.Empty;
            Points = 0;
            Kills = 0;
            Team = 0;
        }
    }
}

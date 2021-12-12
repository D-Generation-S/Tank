using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Events.Data
{
    class PlayerStatistic
    {
        public string Name { get; }
        public int Team { get; }
        public bool Alive { get; }
        public int Points { get; }
        public int Kills { get; }

        public PlayerStatistic(string name, int team, bool alive, int points, int kills)
        {
            Name = name;
            Team = team;
            Alive = alive;
            Points = points;
            Kills = kills;
        }


    }
}

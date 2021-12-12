using System;
using System.Collections.Generic;
using System.Text;
using Tank.Events.Data;

namespace Tank.Events.StateEvents
{
    class GameOverEvent : BaseEvent
    {
        public List<PlayerStatistic> PlayerStatistics;

        public GameOverEvent()
        {
            PlayerStatistics = new List<PlayerStatistic>();
        }
    }
}

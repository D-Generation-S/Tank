using System.Collections.Generic;
using Tank.Events.Data;
using TankEngine.EntityComponentSystem.Events;

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

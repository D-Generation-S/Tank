using Tank.Enums;
using TankEngine.EntityComponentSystem.Events;

namespace Tank.Events
{
    public class GameStateChangedEvent : EntityBasedEvent
    {
        public GameStatesEnum GameStateEnum;
    }
}

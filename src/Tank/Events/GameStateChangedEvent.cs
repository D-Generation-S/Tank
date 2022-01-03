using Tank.Enums;

namespace Tank.Events
{
    public class GameStateChangedEvent : EntityBasedEvent
    {
        public GameStatesEnum GameStateEnum;
    }
}

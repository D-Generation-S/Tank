using System;
using System.Collections.Generic;
using System.Text;
using Tank.Enums;

namespace Tank.Events
{
    class GameStateChangedEvent : EntityBasedEvent
    {
        public GameStatesEnum GameStateEnum;
    }
}

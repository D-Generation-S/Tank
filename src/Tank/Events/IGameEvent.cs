using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Events
{
    interface IGameEvent
    {
        Type Type { get; }

        void Init();
    }
}

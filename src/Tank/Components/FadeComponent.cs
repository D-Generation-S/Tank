using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Components
{
    class FadeComponent : BaseComponent
    {
        public int TargetOpacity;
        public int StartOpacity;
        public float OpacityChange;
        public float RealOpacityChange;
        public int TicksToLive;


        public override void Init()
        {
            TargetOpacity = 100;
            StartOpacity = 0;
            OpacityChange = 0.5f;
            RealOpacityChange = 0;
            TicksToLive = 60;
        }
    }
}

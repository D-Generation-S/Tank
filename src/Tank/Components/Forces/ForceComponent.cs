using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.Enums;

namespace Tank.Components.Forces
{
    internal class ForceComponent : BaseComponent
    {
        public float ForceRadius { get; }

        public float ForceBaseStrengh { get; }

        public ForceTypeEnum ForceType { get; }

        public ForceTriggerTimeEnum ForceTrigger { get; }

        public ForceComponent(
            float forceRadius,
            float forceBaseStrengh,
            ForceTypeEnum forceType,
            ForceTriggerTimeEnum forceTrigger
            )
        {
            allowMultiple = true;
            ForceRadius = forceRadius;
            ForceBaseStrengh = forceBaseStrengh;
            ForceType = forceType;
            ForceTrigger = forceTrigger;
        }

    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.Enums;

namespace Tank.Components.Forces
{
    internal class ForceComponent : BaseComponent
    {
        public float ForceRadius { get; set; }

        public float ForceBaseStrenght { get; set; }

        public ForceTypeEnum ForceType { get; set; }

        public ForceTriggerTimeEnum ForceTrigger { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            ForceRadius = 0;
            ForceBaseStrenght = 0;
            ForceType = ForceTypeEnum.None;
            ForceTrigger = ForceTriggerTimeEnum.None;
        }
    }
}

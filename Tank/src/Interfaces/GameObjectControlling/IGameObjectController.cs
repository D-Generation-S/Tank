using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Interfaces.GameObjectControlling
{
    interface IGameObjectController
    {
        /// <summary>
        /// The key used to fire the canon
        /// </summary>
        Keys Fire { get; }

        /// <summary>
        /// The key used to increase the strenght
        /// </summary>
        Keys StrenghtUp { get; }

        /// <summary>
        /// The key to lower the strenght
        /// </summary>
        Keys StrenghtDown { get; }

        /// <summary>
        /// Rotate the barrel up
        /// </summary>
        Keys RotationUp { get; }

        /// <summary>
        /// Rotate the battel down
        /// </summary>
        Keys RotationDown { get; }
    }
}

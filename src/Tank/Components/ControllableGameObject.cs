using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Components
{
    /// <summary>
    /// This class will make a game object controllable
    /// </summary>
    class ControllableGameObject : BaseComponent
    {
        /// <summary>
        /// The rotation of the barrel
        /// </summary>
        public float BarrelRotation;

        /// <summary>
        /// The strenght to fire the next round
        /// </summary>
        public float Strenght;
    }
}

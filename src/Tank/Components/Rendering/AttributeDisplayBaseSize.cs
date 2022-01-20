using Microsoft.Xna.Framework;
using TankEngine.EntityComponentSystem.Components;

namespace Tank.Components.Rendering
{
    /// <summary>
    /// Class for attributer displays to calculate source size width
    /// </summary>
    internal class AttributeDisplayBaseSize : BaseComponent
    {
        /// <summary>
        /// The base size of the attribute display
        /// </summary>
        public Rectangle BaseSize { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            BaseSize = Rectangle.Empty;
        }
    }
}

using Microsoft.Xna.Framework.Graphics;

namespace TankEngine.EntityComponentSystem.Components.Rendering
{
    /// <summary>
    /// Camera component for camera based rendering
    /// </summary>
    public class CameraComponent : BaseComponent
    {
        /// <summary>
        /// Is the camera active right now
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Take a screenshot of the next draw call
        /// </summary>
        public bool TakeScreenshot { get; set; }

        /// <summary>
        /// The priority of the camera, if there are multiple which are active use the one with the most priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Post processing effects to apply to the image of the camera
        /// </summary>
        public Effect[] PostProcessingEffects { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            Active = false;
            Priority = int.MinValue;
            PostProcessingEffects = new Effect[0];
        }
    }
}

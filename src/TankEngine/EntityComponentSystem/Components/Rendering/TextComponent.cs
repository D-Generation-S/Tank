using Microsoft.Xna.Framework.Graphics;

namespace TankEngine.EntityComponentSystem.Components.Rendering
{
    /// <summary>
    /// Class for a text component to render
    /// </summary>
    public class TextComponent : AbstractRenderingComponent
    {
        /// <summary>
        /// The text to use
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The font to use for rendering
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// The scale of the text to render to
        /// </summary>
        public float Scale { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            base.Init();
            Text = string.Empty;
            Font = null;
            Scale = 1;
        }
    }
}

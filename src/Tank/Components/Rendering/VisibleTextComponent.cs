using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tank.Components.Rendering
{
    /// <summary>
    /// Text render component
    /// </summary>
    [Obsolete("Use the TextComponent instead")]
    class VisibleTextComponent : BaseVisibleComponent
    {
        /// <summary>
        /// The text to show
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The font to use
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// The scale of the text
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public VisibleTextComponent()
        {
            Scale = 1f;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Components.Rendering
{
    internal abstract class BaseVisibleComponent : BaseComponent
    {
        /// <summary>
        /// The draw color for the visible
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The effect to use
        /// </summary>
        public SpriteEffects Effect;

        /// <summary>
        /// The depth of the layer
        /// </summary>
        public float LayerDepth;

        public override void Init()
        {
            Color = Color.White;
            LayerDepth = 1f;
            Effect = SpriteEffects.None;
        }
    }
}

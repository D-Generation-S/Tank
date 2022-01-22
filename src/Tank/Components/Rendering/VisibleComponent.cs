using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tank.Components.Rendering
{
    /// <summary>
    /// This component will make a component visible in the gameworld
    /// </summary>
    [Obsolete("Use core library TextureComponent")]
    class VisibleComponent : BaseVisibleComponent
    {
        /// <summary>
        /// Public access to the texture
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// public access to the draw position
        /// </summary>
        public Rectangle Destination;

        /// <summary>
        /// Single texture size
        /// </summary>
        public Rectangle SingleTextureSize;

        /// <summary>
        /// Public access to the draw position in the sprite
        /// </summary>
        public Rectangle Source;

        /// <summary>
        /// Draw sprite to the middle
        /// </summary>
        public bool DrawMiddle;

        /// <summary>
        /// The cutoff to the right
        /// </summary>
        public int CutoffRight;

        public override void Init()
        {
            base.Init();
            Texture = null;
            Destination = Rectangle.Empty;
            SingleTextureSize = Rectangle.Empty;
            Source = Rectangle.Empty;
            DrawMiddle = false;
            CutoffRight = 0;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    /// <summary>
    /// A visible ui element
    /// </summary>
    abstract class VisibleUiElement : UiElement
    {
        /// <summary>
        /// The texture to use
        /// </summary>
        protected readonly SpriteSheet textureToShow;

        /// <summary>
        /// The spritebatch to use for drawing
        /// </summary>
        protected readonly SpriteBatch spriteBatch;

        /// <summary>
        /// The text to display
        /// </summary>
        protected string text;

        /// <summary>
        /// The font to use
        /// </summary>
        protected SpriteFont font;

        /// <summary>
        /// The collider of the element for interaction
        /// </summary>
        protected Rectangle collider;

        /// <summary>
        /// any offset in rendering
        /// </summary>
        protected Vector2 renderOffset;

        /// <summary>
        /// The sound to make if clicked
        /// </summary>
        protected SoundEffect clickSound;

        /// <summary>
        /// Instance of the click sound
        /// </summary>
        protected SoundEffectInstance clickSoundInstance;

        /// <summary>
        /// The sound to make on hover
        /// </summary>
        protected SoundEffect hoverSound;

        /// <summary>
        /// Instance of the hover sound
        /// </summary>
        protected SoundEffectInstance hoverSoundInstance;

        /// <summary>
        /// The effect volume to use
        /// </summary>
        protected float effectVolume;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="position">The position to place</param>
        /// <param name="width">The width of the element</param>
        /// <param name="textureToShow">The texture to use</param>
        /// <param name="spriteBatch">The spritebatch for drawing</param>
        public VisibleUiElement(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spriteBatch) : base(position, width)
        {
            this.textureToShow = textureToShow;
            this.spriteBatch = spriteBatch;
            text = string.Empty;
            effectVolume = 1.0f;

            SetupTextures();
            Setup();
            UpdateCollider();

        }

        /// <inheritdoc/>
        protected virtual void Setup()
        {
        }

        /// <summary>
        /// Set the render offset
        /// </summary>
        /// <param name="renderOffset">The position to offset</param>
        public void SetRenderOffset(Vector2 renderOffset)
        {
            this.renderOffset = renderOffset;
        }

        /// <inheritdoc/>
        protected abstract void SetupTextures();

        /// <inheritdoc/>
        protected abstract void UpdateCollider();

        /// <summary>
        /// Set the effect volume
        /// </summary>
        /// <param name="volume">The volume to set</param>
        public virtual void SetEffectVolume(float volume)
        {
            effectVolume = volume;
        }

        /// <summary>
        /// Set the click effect
        /// </summary>
        /// <param name="clickSound">The click sound</param>
        public virtual void SetClickEffect(SoundEffect clickSound)
        {
            if (clickSound == null)
            {
                return;
            }
            this.clickSound = clickSound;
            clickSoundInstance = clickSound.CreateInstance();
        }

        /// <summary>
        /// Set the hover effect
        /// </summary>
        /// <param name="hoverSound">The hover sound</param>
        public virtual void SetHoverEffect(SoundEffect hoverSound)
        {
            if (hoverSound == null)
            {
                return;
            }
            this.hoverSound = hoverSound;
            hoverSoundInstance = hoverSound.CreateInstance();
        }

        /// <inheritdoc/>
        public virtual void SetText(string text)
        {
            this.text = text;
            UpdateCollider();
        }

        /// <inheritdoc/>
        public virtual void SetFont(SpriteFont font)
        {
            this.font = font;
        }

        /// <summary>
        /// Convert the text to pixel lenght
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>The size of the text</returns>
        protected Vector2 GetTextLenght(string text)
        {
            return GetTextLenght(text, font);
        }

        /// <summary>
        /// Convert the text to pixel lenght
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <param name="font">The font to use</param>
        /// <returns>The size of the text</returns>
        protected Vector2 GetTextLenght(string text, SpriteFont font)
        {
            if (font == null || text == null) 
            {
                return Vector2.Zero;
            }
            return font.MeasureString(text);
        }

        /// <inheritdoc/>
        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position + renderOffset);
            UpdateCollider();
        }
    }
}

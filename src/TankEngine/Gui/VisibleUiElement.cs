using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using TankEngine.DataStructures.Spritesheet;

namespace TankEngine.Gui
{
    /// <summary>
    /// A visible ui element
    /// </summary>
    public abstract class VisibleUiElement : UiElement
    {
        /// <summary>
        /// The tag to search for the left area part
        /// </summary>
        protected static string LEFT_TAG = "left";

        /// <summary>
        /// The tag to search for the center area part
        /// </summary>
        protected static string CENTER_TAG = "center";

        /// <summary>
        /// The tag to search for the right area part
        /// </summary>
        protected static string RIGHT_TAG = "right";

        /// <summary>
        /// The texture to use
        /// </summary>
        protected readonly SpritesheetTexture spritesheetTexture;

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
        /// The base filter to use for searching areas in spritesheet
        /// </summary>
        protected readonly string baseFilter;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="position">The position to place</param>
        /// <param name="width">The width of the element</param>
        /// <param name="spritesheetTexture">The texture to use</param>
        /// <param name="spriteBatch">The spritebatch for drawing</param>
        public VisibleUiElement(Vector2 position, int width, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, string baseFilter) : base(position, width)
        {
            this.spritesheetTexture = spritesheetTexture;
            this.spriteBatch = spriteBatch;
            text = string.Empty;
            effectVolume = 1.0f;
            this.baseFilter = baseFilter;

            SetupAreas();
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

        /// <summary>
        /// Search data by property value
        /// </summary>
        /// <param name="value">The value to search for</param>
        /// <returns>True if there is any value with the searched value</returns>
        protected Func<SpritesheetProperty, bool> SearchByPropertyValue(string value)
        {
            return property => property.Value.ToLower() == value.ToLower();
        }

        /// <inheritdoc/>
        protected abstract void SetupAreas();

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
            return GetTextLength(text, font);
        }

        /// <summary>
        /// Convert the text to pixel lenght
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <param name="font">The font to use</param>
        /// <returns>The size of the text</returns>
        protected Vector2 GetTextLength(string text, SpriteFont font)
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

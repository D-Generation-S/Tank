using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    abstract class VisibleUiElement : UiElement
    {
        protected readonly SpriteSheet textureToShow;
        protected readonly SpriteBatch spriteBatch;
        protected string text;
        protected SpriteFont font;
        protected Rectangle collider;
        protected Vector2 renderOffset;

        protected SoundEffect clickSound;
        protected SoundEffectInstance clickSoundInstance;
        protected SoundEffect hoverSound;
        protected SoundEffectInstance hoverSoundInstance;

        public VisibleUiElement(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spriteBatch) : base(position, width)
        {
            this.textureToShow = textureToShow;
            this.spriteBatch = spriteBatch;
            text = string.Empty;

            SetupTextures();
            Setup();
            UpdateCollider();

        }

        protected virtual void Setup()
        {
        }

        public void SetRenderOffset(Vector2 renderOffset)
        {
            this.renderOffset = renderOffset;
        }

        protected abstract void SetupTextures();

        protected abstract void UpdateCollider();

        public virtual void SetClickEffect(SoundEffect clickSound)
        {
            if (clickSound == null)
            {
                return;
            }
            this.clickSound = clickSound;
            clickSoundInstance = clickSound.CreateInstance();
        }

        public virtual void SetHoverEffect(SoundEffect hoverSound)
        {
            if (hoverSound == null)
            {
                return;
            }
            this.hoverSound = hoverSound;
            hoverSoundInstance = hoverSound.CreateInstance();
        }

        public virtual void SetText(string text)
        {
            this.text = text;
            UpdateCollider();
        }

        public virtual void SetFont(SpriteFont font)
        {
            this.font = font;
        }

        protected Vector2 GetTextLenght(string text, SpriteFont font)
        {
            if (font == null || text == null) 
            {
                return Vector2.Zero;
            }
            return font.MeasureString(text);
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position + renderOffset);
            UpdateCollider();
        }
    }
}

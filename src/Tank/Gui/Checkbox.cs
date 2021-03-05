using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataStructure;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    class Checkbox : VisibleUiElement
    {
        private MouseState lastMouseState;

        public bool Checked;

        private Rectangle uncheckedSource;
        private Rectangle uncheckedHoverSource;
        private Rectangle checkedSource;
        private Rectangle checkedHoverSource;
        private Rectangle sourceToDraw;

        private readonly int boxTextSpace;

        private Position imageSize;

        public Checkbox(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spriteBatch) : base(position, width, textureToShow, spriteBatch)
        {
            boxTextSpace = 10;
        }

        /// <inheritdoc/>
        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
            UpdateCollider();
        }

        /// <inheritdoc/>
        protected override void SetupTextures()
        {
            imageSize = textureToShow.GetPatternImageSize("CheckboxUnchecked");
            uncheckedSource = textureToShow.GetAreaFromPatternName("CheckboxUnchecked");
            uncheckedHoverSource = textureToShow.GetAreaFromPatternName("CheckboxHoverUnchecked");
            checkedSource = textureToShow.GetAreaFromPatternName("CheckboxChecked");
            checkedHoverSource = textureToShow.GetAreaFromPatternName("CheckboxHoverChecked");

            sourceToDraw = uncheckedSource;
        }

        /// <inheritdoc/>
        protected override void UpdateCollider()
        {
            collider = new Rectangle((int)Position.X, (int)Position.Y, imageSize.X + boxTextSpace + (int)GetTextLenght(text, font).X, imageSize.Y);
            Size = collider.Size.ToVector2();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            sourceToDraw = Checked ? checkedSource : uncheckedSource;

            if (collider.Contains(GetMousePosition()))
            {
                sourceToDraw = Checked ? checkedHoverSource : uncheckedHoverSource;
                if (hoverSound != null
                    && !collider.Contains(mouseWrapper.GetPosition(lastMouseState.Position))
                    && hoverSound.State != SoundState.Playing)
                {
                    hoverSound.Play();
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    if (clickSound != null && clickSound.State != SoundState.Playing)
                    {
                        clickSound.Play();
                    }
                    Checked = !Checked;
                }
            }

            lastMouseState = Mouse.GetState();
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(
                textureToShow.CompleteImage,
                new Rectangle((int)Position.X, (int)Position.Y, imageSize.X, imageSize.Y),
                sourceToDraw,
                Color.White
                );
            if (font == null)
            {
                return;
            }
            Vector2 textPosition = Position;
            textPosition.X += imageSize.X + boxTextSpace;
            textPosition.Y += imageSize.Y / 2;
            textPosition.Y -= GetTextLenght(text, font).Y / 2;
            spriteBatch.DrawString(font, text, textPosition, Color.White);
        }


    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tank.DataStructure;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    /// <summary>
    /// Simple checkbox class
    /// </summary>
    class Checkbox : VisibleUiElement
    {
        /// <summary>
        /// The last known mouse state
        /// </summary>
        private MouseState lastMouseState;

        /// <summary>
        /// Is the box currently checked
        /// </summary>
        public bool Checked;

        /// <summary>
        /// The area to use if not checked and not hovered
        /// </summary>
        private Rectangle uncheckedSource;

        /// <summary>
        /// The area to use if not checked but hovered
        /// </summary>
        private Rectangle uncheckedHoverSource;
        
        /// <summary>
        /// The area to use if checked but not hovered
        /// </summary>
        private Rectangle checkedSource;

        /// <summary>
        /// The area to use if checked and hovered
        /// </summary>
        private Rectangle checkedHoverSource;

        /// <summary>
        /// The current source to draw
        /// </summary>
        private Rectangle sourceToDraw;

        /// <summary>
        /// The space between the checkbox and the text
        /// </summary>
        private readonly int boxTextSpace;

        /// <summary>
        /// The size of a single image
        /// </summary>
        private Position imageSize;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position to place this element</param>
        /// <param name="width">The width of the element</param>
        /// <param name="textureToShow">The texture to use</param>
        /// <param name="spriteBatch">The spritebatch for drawing call</param>
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
            uncheckedSource = textureToShow.GetAreaFromPattern("CheckboxUnchecked");
            uncheckedHoverSource = textureToShow.GetAreaFromPattern("CheckboxHoverUnchecked");
            checkedSource = textureToShow.GetAreaFromPattern("CheckboxChecked");
            checkedHoverSource = textureToShow.GetAreaFromPattern("CheckboxHoverChecked");

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
                    && hoverSoundInstance.State != SoundState.Playing)
                {
                    hoverSound.Play(effectVolume, 0f, 0f);
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    if (clickSound != null && clickSoundInstance.State != SoundState.Playing)
                    {
                        clickSound.Play(effectVolume, 0f, 0f);
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

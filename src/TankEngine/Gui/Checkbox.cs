using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using TankEngine.DataStructures.Spritesheet;

namespace TankEngine.Gui
{
    /// <summary>
    /// Simple checkbox class
    /// </summary>
    public class Checkbox : VisibleUiElement
    {
        /// <summary>
        /// The default filter to use to get checkbox areas
        /// </summary>
        private const string DEFAULT_FILTER = "checkbox";

        /// <summary>
        /// Search value for image of idle checkbox
        /// </summary>
        private const string IDLE = "idle";

        /// <summary>
        /// Search value for image of activ checkbox
        /// </summary>
        private const string ACTIVE = "active";

        /// <summary>
        /// Search value for image of checked textbox
        /// </summary>
        private const string CHECKED = "checked";

        /// <summary>
        /// Search value for image of unchcked textbox
        /// </summary>
        private const string UNCHECKED = "unchecked";

        /// <summary>
        /// The last known mouse state
        /// </summary>
        private MouseState lastMouseState;

        /// <summary>
        /// Is the box currently checked
        /// </summary>
        public bool Checked;

        /// <summary>
        /// The areas for the gui element
        /// </summary>
        protected List<SpritesheetArea> Areas;

        /// <summary>
        /// The area to use if not checked and not hovered
        /// </summary>
        private Rectangle idleBox;

        /// <summary>
        /// The area to use if not checked but hovered
        /// </summary>
        private Rectangle idleBoxHover;

        /// <summary>
        /// The area to use if checked but not hovered
        /// </summary>
        private Rectangle activeBox;

        /// <summary>
        /// The area to use if checked and hovered
        /// </summary>
        private Rectangle activeBoxHover;

        /// <summary>
        /// The current source to draw
        /// </summary>
        private Rectangle sourceToDraw;

        /// <summary>
        /// The space between the checkbox and the text
        /// </summary>
        private readonly int boxTextSpace;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position to place this element</param>
        /// <param name="width">The width of the element</param>
        /// <param name="spritesheetTexture">The texture to use</param>
        /// <param name="spriteBatch">The spritebatch for drawing call</param>
        public Checkbox(Vector2 position, int width, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch)
            : this(position, width, spritesheetTexture, spriteBatch, DEFAULT_FILTER)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position to place this element</param>
        /// <param name="width">The width of the element</param>
        /// <param name="spritesheetTexture">The texture to use</param>
        /// <param name="spriteBatch">The spritebatch for drawing call</param>
        /// <param name="baseFilter">The base filter used to get the areas for this element</param>
        public Checkbox(Vector2 position, int width, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, string baseFilter)
            : base(position, width, spritesheetTexture, spriteBatch, baseFilter)
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
        protected override void SetupAreas()
        {
            Areas = spritesheetTexture.Areas.Where(area => area.Properties.Any(SearchByPropertyValue(baseFilter))).ToList();
            idleBox = GetAreaByValue(IDLE, UNCHECKED);
            idleBoxHover = GetAreaByValue(ACTIVE, UNCHECKED);
            activeBox = GetAreaByValue(IDLE, CHECKED);
            activeBoxHover = GetAreaByValue(ACTIVE, CHECKED);

            sourceToDraw = idleBox;
        }

        private Rectangle GetAreaByValue(string value, string state)
        {
            SpritesheetArea centerArea = Areas.FirstOrDefault(area => area.ContainsPropertyValue(value, false) && area.ContainsPropertyValue(state, false));
            return centerArea == null ? Rectangle.Empty : centerArea.Area;
        }

        /// <inheritdoc/>
        protected override void UpdateCollider()
        {
            float totalSizeX = idleBox.Width + boxTextSpace + GetTextLength(text, font).X;
            collider = new Rectangle((int)Position.X, (int)Position.Y, (int)totalSizeX, idleBox.Width);

            Size = new Vector2(totalSizeX, MathHelper.Max(GetTextLength(text, font).Y, idleBox.Height));
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            sourceToDraw = Checked ? activeBox : idleBox;

            if (collider.Contains(GetMousePosition()))
            {
                sourceToDraw = Checked ? activeBoxHover : idleBoxHover;
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
                spritesheetTexture.Texture,
                new Rectangle((int)Position.X, (int)Position.Y, sourceToDraw.Width, sourceToDraw.Height),
                sourceToDraw,
                Color.White
                );
            if (font == null)
            {
                return;
            }
            Vector2 textPosition = Position;
            textPosition.X += idleBox.Width + boxTextSpace;
            textPosition.Y += idleBox.Height / 2;
            textPosition.Y -= GetTextLength(text, font).Y / 2;
            spriteBatch.DrawString(font, text, textPosition, Color.White);
        }
    }
}

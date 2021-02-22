using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Tank.Code.GUIClasses
{
    public class GUIButton : GUIPrimitiv
    {
        private Texture2D t2dMainTexture;
        private Texture2D t2dHoverTexture;
        private string _Text;
        public string Text
        {
            set
            {
                _Text = value;
            }
        }
        private SpriteFont TextFont;
        private SoundEffect ClickSound;

        public event EventHandler Click;

        private bool MouseClicked;

        public Color TextColor;

        public GUIButton(int PositionX, int PositionY, int Width, int Height, GraphicsDevice graphicDevice, Texture2D Texture, string ButtonText, SpriteFont Font, Texture2D HoverTexture = null, SoundEffect Click = null, bool Overlayer = false)
            : base(PositionX, PositionY, Width, Height, Texture, Overlayer)
        {
            if (HoverTexture == null)
                t2dHoverTexture = Texture;
            else
                t2dHoverTexture = HoverTexture;
            TextFont = Font;
            Text = ButtonText;
            TextColor = Color.Black;
            t2dMainTexture = Texture;

            ClickSound = Click;

            if (TextFont.MeasureString(_Text).X > Size.X || TextFont.MeasureString(_Text).Y > Size.Y)
            {
                Dispose();
                throw new ArgumentException("TankGame.OutOfBoundsException: Text ist to big for button.");
            }
        }

        public override void Draw(SpriteBatch SB)
        {
            if (_hidden)
                return;
            base.Draw(SB);
            Vector2 StringSize = TextFont.MeasureString(_Text);
            Vector2 StringPosition = new Vector2(Position.X + Size.X / 2 - StringSize.X / 2, Position.Y + Size.Y / 2 - StringSize.Y / 2);
            SB.DrawString(TextFont, _Text, StringPosition, TextColor);
        }

        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime currentGameTime, GamePadState gsState)
        {
            if (_hidden)
                return;
            if (Element.Contains(MouseHandler.Position))
            {
                CurrentTexture = t2dHoverTexture;
                if (!MouseClicked && MouseHandler.LeftButton == ButtonState.Pressed)
                {
                    MouseClicked = true;
                }
                if (MouseClicked && MouseHandler.LeftButton == ButtonState.Released)
                {
                    Click?.Invoke(this, new EventArgs());

                    if (ClickSound != null)
                        ClickSound.Play();
                    MouseClicked = false;
                }
            }
            else
            {
                if (MouseClicked && MouseHandler.LeftButton == ButtonState.Released)
                    MouseClicked = false;
                CurrentTexture = t2dMainTexture;
            }
        }
    }
}

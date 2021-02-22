using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Tank.Enums;

namespace Tank.Code.GUIClasses
{
    public class GUIToggle : GUIPrimitiv
    {
        Texture2D t2dOnTexture;
        Texture2D t2dOffTexture;

        private bool _checked;
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
                if (_checked)
                    CurrentTexture = t2dOnTexture;
                else
                    CurrentTexture = t2dOffTexture;
            }
        }

        private bool HoverClick;

        private string _Text;
        public string Text
        {
            set
            {
                _Text = value;
            }
        }
        private SpriteFont TextFont;
        public Color TextColor;

        SoundEffect ToggleSound;

        public PossibleTextPosition TextPosition;

        public event EventHandler Changing;

        public GUIToggle(int PositionX, int PositionY, int Width, int Height, Texture2D TextureSwitchedOn, Texture2D TextureSwitchedOff, string GUIText, SpriteFont Font, SoundEffect SwitchSound = null, PossibleTextPosition TargetTextPosition = PossibleTextPosition.Left, bool Overlayer = false)
            : base(PositionX, PositionY, Width, Height, TextureSwitchedOn, Overlayer)
        {
            TextPosition = TargetTextPosition;
            t2dOnTexture = TextureSwitchedOn;
            t2dOffTexture = TextureSwitchedOff;
            ToggleSound = SwitchSound;
            _checked = false;
            Text = GUIText;
            TextFont = Font;
            CurrentTexture = t2dOffTexture;
            TextColor = Color.Black;
        }

        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime currentGameTime, GamePadState gsState)
        {
            if (_hidden)
                return;
            if (Element.Contains(MouseHandler.Position))
            {
                if (!HoverClick && MouseHandler.LeftButton == ButtonState.Pressed)
                {
                    HoverClick = true;
                }
                if (HoverClick && MouseHandler.LeftButton == ButtonState.Released)
                {
                    if (ToggleSound != null)
                        ToggleSound.Play();
                    _checked = !_checked;
                    if (_checked)
                        CurrentTexture = t2dOnTexture;
                    else
                        CurrentTexture = t2dOffTexture;
                    HoverClick = false;
                    Changing?.Invoke(this, new GUIEventArgs(_checked));
                }
            }
            else
            {
                if (HoverClick && MouseHandler.LeftButton == ButtonState.Released)
                    HoverClick = false;
            }
        }

        public override void Draw(SpriteBatch SB)
        {
            if (_hidden)
                return;
            base.Draw(SB);
            Vector2 CurrentTextPosition = new Vector2(0, 0);
            int PlaceHolder = 10;
            Vector2 StringSize = TextFont.MeasureString(_Text);
            switch (TextPosition)
            {
                case PossibleTextPosition.Left:
                    CurrentTextPosition = new Vector2(Position.X - StringSize.X - PlaceHolder, Position.Y + Size.Y / 2 - StringSize.Y / 2);
                    break;
                case PossibleTextPosition.Right:
                    CurrentTextPosition = new Vector2(Position.X + Size.X + PlaceHolder, Position.Y + Size.Y / 2 - StringSize.Y / 2);
                    break;
                default:
                    break;
            }
            SB.DrawString(TextFont, _Text, CurrentTextPosition, TextColor);
        }
    }
}

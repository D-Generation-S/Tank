using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tank.Code.GUIClasses
{
    public class GUISlider : GUIPrimitiv
    {
        private bool _isInitialized;
        private MouseState _previousMouseState;
        public int Value
        {
            get;
            set;
        }
        public float MaxValue
        {
            get;
            set;
        }
        public Texture2D SliderTexture
        {
            get;
            set;
        }
        public int SliderWidth
        {
            get; set;
        }
        public int SliderHeight
        {
            get; set;
        }
        public int SliderOffsetX
        {
            get; set;
        }
        public int SliderOffsetY
        {
            get; set;
        }
        public Rectangle InnerElement
        {
            get;
            private set;
        }

        public event EventHandler OnChange;

        public GUISlider()
            : base()
        {
            _isInitialized = false;
        }

        public GUISlider(int x, int y, int Width, int Height)
            : base(x, y, Width, Height)
        {
            _isInitialized = true;
        }

        public void Initialize()
        {
            if (CurrentTexture != null)
            {
                if (Position == null)
                {
                    Position = Vector2.Zero;
                }
                if (Element == null)
                    Element = new Rectangle((int)Position.X, (int)Position.Y, CurrentTexture.Width, CurrentTexture.Height);
                InnerElement = new Rectangle((int)Position.X + SliderOffsetX, (int)Position.Y, CurrentTexture.Width - (SliderOffsetX * 2) - (SliderTexture.Width / 2), CurrentTexture.Height);
                _isInitialized = SliderTexture != null;
            }
            else
                _isInitialized = false;
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState, GameTime currentGameTime, GamePadState gsState)
        {
            if (_isInitialized)
            {

                if (InnerElement.Contains(MouseHandler.Position))
                {
                    if (MouseHandler.LeftButton == ButtonState.Pressed)
                        Value = (int)((MouseHandler.Position.X - InnerElement.X) / ((float)InnerElement.Width / 100));
                }
                if (MouseHandler.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
                    OnChange?.Invoke(this, new GUIEventArgs(Value));

                base.Update(mouseState, keyboardState, currentGameTime, gsState);
                _previousMouseState = mouseState;
            }
        }

        public override void Draw(SpriteBatch SB)
        {
            if (_isInitialized)
            {
                if (_hidden)
                    return;
                base.Draw(SB);
                int sliderPosX = (int)(InnerElement.X + (Value * (InnerElement.Width) / 100));
                SB.Draw(SliderTexture, new Vector2(sliderPosX, Position.Y + SliderOffsetY), Color.White);
            }
        }
    }
}

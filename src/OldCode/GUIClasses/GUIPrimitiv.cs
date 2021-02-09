using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tank.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace Tank.Code.GUIClasses
{

    public class GUIEventArgs : EventArgs
    {
        public object Value
        {
            get; set;
        }
        public GUIEventArgs(object value)
        {
            Value = value;
        }
    }
    public enum TextPosition
    {
        Left,
        Center,
        Right,
    }

    public class GUIPrimitiv : IRenderObj, IActiveObj, IDisposable
    {
        public string Name
        {
            get; set;
        }

        private Texture2D t2dCurrentTexture;
        public Texture2D CurrentTexture
        {
            get
            {
                return t2dCurrentTexture;
            }
            set
            {
                t2dCurrentTexture = value;
            }
        }
        public Color[] CurrentTexturecolorData
        {
            get
            {
                Color[] colorData = new Color[t2dCurrentTexture.Width + t2dCurrentTexture.Height];
                t2dCurrentTexture.GetData<Color>(colorData);
                return colorData;
            }
        }

        internal Rectangle Element;
        public Vector2 Position
        {
            get
            {
                return Element.Location.ToVector2();
            }
            set
            {
                Element.Location = value.ToPoint();
            }
        }
        public Vector2 Size
        {
            get
            {
                return Element.Size.ToVector2();
            }
            set
            {
                Element.Size = value.ToPoint();
            }
        }

        private GraphicsDevice GD;
        private Color DrawColor;
        public Color TextureColor
        {
            set
            {
                DrawColor = value;
            }
        }

        public bool Active
        {
            get;
            set;
        }

        protected bool _hidden;
        public bool Hidden
        {
            set
            {
                _hidden = value;
            }
        }

        public object InternalData;

        public GUIPrimitiv(int PositionX, int PositionY, int Width, int Height, Texture2D Texture = null, bool Overlayer = false)
        {
            Element = new Rectangle(PositionX, PositionY, Width, Height);
            GD = TankGame.PublicGraphicsDevice;
            if (Texture != null)
                t2dCurrentTexture = Texture;
            else
                t2dCurrentTexture = CreateTexture();
            DrawColor = Color.White;
            ActiveHandler.Instance.Add(this);
            if (Overlayer)
                Renderer.Instance.overlayerObjects.Add(this);
            else
                Renderer.Instance.Add(this);
        }

        public GUIPrimitiv()
        {
            Element = new Rectangle(0, 0, 100, 100);
            GD = TankGame.PublicGraphicsDevice;
            t2dCurrentTexture = CreateTexture();
            DrawColor = Color.White;
            ActiveHandler.Instance.Add(this);
            Renderer.Instance.Add(this);
        }

        internal Texture2D CreateTexture()
        {
            Color[] colorData = new Color[1];
            colorData[0] = Color.White;
            Texture2D NewTexture = new Texture2D(GD, 1, 1);
            NewTexture.SetData<Color>(colorData);
            return NewTexture;
        }


        public virtual void Draw(SpriteBatch SB)
        {
            if (!_hidden && t2dCurrentTexture != null)
                SB.Draw(t2dCurrentTexture, Element, DrawColor);
        }

        public virtual void Update(MouseState mouseState, KeyboardState keyboardState, GameTime currentGameTime, GamePadState gsState)
        {
            if (_hidden)
                return;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Renderer.Instance.Remove(this);
                ActiveHandler.Instance.Remove(this);
            }
        }

        ~GUIPrimitiv()
        {
            Dispose(false);
        }
    }
}

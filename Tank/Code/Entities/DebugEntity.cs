using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tank.Code.Entities
{
    public class DebugEntity : IRenderObj, IActiveObj, IDisposable
    {
        public float x
        {
            get;
            set;
        }

        public float y
        {
            get;
            set;
        }

        public bool Active
        {
            get;
            set;
        }

        private string _description;
        private float _ttl;
        private Texture2D _drawTexture;

        private float _elapsedTime;
        private int _sizeMultiplier;

        public DebugEntity(int PosX, int PosY, Texture2D DT, string description = "", int TTL = 2000, int sizeMultiplier = 1)
        {
            x = PosX;
            y = PosY;
            _ttl = TTL;
            _description = description;
            _drawTexture = DT;
            _sizeMultiplier = sizeMultiplier;

            Renderer.Instance.Add(this);
            ActiveHandler.Instance.Add(this);
        }

        virtual public void Draw(SpriteBatch sb)
        {
            if (_drawTexture != null)
            {
                int OffsetX = (int)(x - _drawTexture.Width / 2f);
                int OffsetY = (int)(y - _drawTexture.Height / 2f);
                sb.Draw(_drawTexture, new Rectangle(OffsetX, OffsetY, _drawTexture.Width * _sizeMultiplier, _drawTexture.Height * _sizeMultiplier), Color.White);
                if (!string.IsNullOrEmpty(_description))
                    sb.DrawString(Settings.GlobalFont, _description, new Vector2(x + 20, y - 20), Color.Black);
            }
                
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

        public void Update(MouseState mouseState, KeyboardState keyboardState, GameTime currentGameTime, GamePadState gsState)
        {
            float currentTime = (float)(currentGameTime.TotalGameTime.TotalSeconds * 1000 + currentGameTime.TotalGameTime.TotalMilliseconds);
            if (_elapsedTime <= 0)
                _elapsedTime = currentTime;
            _ttl -= currentTime - _elapsedTime;
            _elapsedTime = currentTime;

            if (_ttl <= 0)
                Dispose();
        }

        ~DebugEntity()
        {
            Dispose(false);
        }
    }
}

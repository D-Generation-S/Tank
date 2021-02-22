using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tank.Interfaces;

namespace Tank.Code.GUIClasses
{
    public class PlayerOverlay : IRenderObj, IDisposable
    {
        public float CurrentPower
        {
            get; set;
        }
        public float CurrentAngle
        {
            get; set;
        }
        public int Index
        {
            get;
            set;
        }

        private float _health;
        private int screenWidth;
        private int screenHeight;
        private int screenMargin;
        private Texture2D t2DMeter;
        private Texture2D t2DBar;
        private Texture2D t2DAngle;
        private Texture2D t2DAngleContainer;

        private float meterX;
        private float meterY;
        private float barOffset;

        private float angleX;
        private float angleY;

        public List<Vector2> CurrentBulletHitPoint;

        public PlayerOverlay(int index, int margin = 10)
        {
            screenMargin = margin;
            Index = index;

            screenWidth = Settings.MaxWindowSize.Width;
            screenHeight = Settings.MaxWindowSize.Height;

            Initialize();
            LoadContent();
        }

        public void LoadContent()
        {
            t2DMeter = Settings.Meter;
            t2DBar = Settings.Bar;
            barOffset = t2DMeter.Width - t2DBar.Width;
            meterX = screenWidth - screenMargin - t2DMeter.Width;
            meterY = screenHeight - screenMargin - t2DMeter.Height;


            t2DAngleContainer = Settings.PlayerAngleBackground;
            t2DAngle = Settings.PlayerAngleForeground;
            angleX = screenMargin;
            angleY = screenHeight - screenMargin - t2DAngleContainer.Height;
        }

        public void Initialize()
        {
            Renderer.Instance.Add(this);
        }

        public void Draw(SpriteBatch sb)
        {
            if (Settings.Debug)
                foreach (Vector2 CurrentPos in CurrentBulletHitPoint)
                    sb.Draw(Settings.DynamicPixelTexture, new Rectangle((int)CurrentPos.X - 5, (int)CurrentPos.Y - 5, 10, 10), Color.Red);

            sb.Draw(t2DMeter, new Vector2(meterX, meterY), Color.White);
            sb.Draw(t2DBar, new Vector2(meterX + barOffset, meterY + t2DMeter.Height - (t2DMeter.Height * (CurrentPower / 100))), Color.White);

            //sb.Draw(t2DAngle, )
            sb.Draw(t2DAngleContainer, new Vector2(angleX, angleY), Color.White);
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
            }
        }

        ~PlayerOverlay()
        {
            Dispose(false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tank.Code.GUIClasses
{
    class GUIProgressBar : GUIPrimitiv
    {
        Texture2D t2dBack;
        private float _progress;
        private Rectangle progressSource;
        private Rectangle progressDesination;
        public int X
        {
            get
            {
                return progressDesination.X;
            }
            set
            {
                Element.X = value;
                progressDesination.X = value;
            }
        }
        public int Y
        {
            get
            {
                return progressDesination.Y;
            }
            set
            {
                Element.Y = value;
                progressDesination.Y = value;
            }
        }
        public float Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                if (value > 1.0f)
                    _progress = 1.0f;
                else if (value < 0.0f)
                    _progress = 0.0f;
                else
                    _progress = value;
                RefreshProgress();
            }
        }

        private void RefreshProgress()
        {
            if (progressSource != null)
            {
                progressDesination.Width = progressSource.Width = (int)(Element.Width * _progress);                
            }
        }

        public GUIProgressBar(int PositionX, int PositionY, int Width, int Height, Texture2D Texture, Texture2D BackTexture, bool Overlayer = false)
            : base(PositionX, PositionY, Width, Height, Texture, Overlayer)
        {
            t2dBack = BackTexture;
            progressDesination = Element;
            progressSource = Element;
            progressSource.X = 0;
            progressSource.Y = 0;
            Progress = 0;
        }

        public override void Draw(SpriteBatch SB)
        {
            base.Draw(SB);
            if (_hidden)
                return;
            SB.Draw(t2dBack, progressDesination, progressSource, Color.White);

        }
    }
}

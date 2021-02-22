using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tank.Interfaces;

namespace Tank.Code
{
    public class Animation : IDisposable, IRenderObj
    {
        public Texture2D spriteSheet
        {
            get; set;
        }
        float time;
        float frameTime = 0.1f;
        int horizontalFrameIndex = 0;
        int verticalFrameIndex = 0;
        int horizontalFrames = 0;
        int verticalFrames = 0;
        int frameHeight = 0;
        int frameWidth = 0;
        private int emptyFrames;
        public float Rotation
        {
            get;
            set;
        }

        public int X
        {
            get; set;
        }
        public int Y
        {
            get; set;
        }

        public Animation(Texture2D spritesheet, int width, int height, float frametime = 0.1f, int empty = 0)
        {
            spriteSheet = spritesheet;
            frameHeight = height;
            frameWidth = width;
            frameTime = frametime;
            horizontalFrames = spriteSheet.Width / frameWidth;
            verticalFrames = spriteSheet.Height / frameHeight;
            emptyFrames = empty;
            Initialize();
        }

        public void Initialize()
        {
            Renderer.Instance.Add(this);
        }

        public void Draw(SpriteBatch sb)
        {
            time += (float)Settings.GameTime.ElapsedGameTime.TotalSeconds;
            while (time > frameTime)
            {
                horizontalFrameIndex++;
                if (horizontalFrameIndex >= horizontalFrames)
                {
                    horizontalFrameIndex = 0;
                    verticalFrameIndex++;
                }
                if (verticalFrameIndex >= verticalFrames)
                    verticalFrameIndex = 0;
                time = 0;
            }
            if ((horizontalFrameIndex + 1) * (verticalFrameIndex + 1) > (horizontalFrames * verticalFrames) - emptyFrames - 1)
            {
                horizontalFrameIndex = 0;
                verticalFrameIndex = 0;
            }

            Rectangle destination = new Rectangle(X, Y, frameWidth, frameHeight);

            Rectangle source = new Rectangle(horizontalFrameIndex * frameWidth, verticalFrameIndex * frameHeight, frameWidth, frameHeight);


            sb.Draw(spriteSheet, destination, source, Color.White, Rotation, new Vector2(frameWidth / 2, frameHeight / 2), SpriteEffects.None, 1f);
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
        ~Animation()
        {
            Dispose(false);
        }
    }
}

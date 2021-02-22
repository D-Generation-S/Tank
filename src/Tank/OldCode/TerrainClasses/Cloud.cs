using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tank.Code.TerrainClasses
{
    public class Cloud
    {
        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get; set;
        }
        private Texture2D Texture;
        private GameTime PreviousGameTime;
        private float x;
        private GraphicsDevice Graphics;
        private Color[] EmptyColorData;

        public Cloud(int width, int height, GraphicsDevice graphics)
        {
            Width = width;
            Height = height;

            Graphics = graphics;
            EmptyColorData = new Color[Width * Height];
            for (int i = 0; i < EmptyColorData.Length; i++)
                EmptyColorData[i] = Color.Transparent;

            Texture = GenerateClouds(10, 0.3f);
        }

        private Texture2D GenerateClouds(int interval, float smooth, float offset = 0.5f, bool linear = false)
        {
            int DrawSteps = Width / interval;
            Texture2D t2DReturn = new Texture2D(Graphics, Width, Height);

            Random rnd = new Random();

            Color[] colorData = new Color[Width * Height];

            int previousX = 0;
            float previousY = (float)(rnd.NextDouble() * (Height * smooth)) + (Height * offset);
            float b = 0.0f;
            for (int i = 1; i <= interval; i++)
            {
                int newX = i * DrawSteps;
                float newY = (float)(rnd.NextDouble() * (Height * smooth)) + (Height * offset);
                if (Settings.ShowPerlinNoise)
                {
                    for (int k = -8; k <= 8; k++)
                    {
                        for (int j = -8; j <= 8; j++)
                        {
                            {
                                int colorIndex = (newX + k) + Width * (int)(newY + j);
                                if (colorIndex < colorData.Length && colorIndex >= 0)
                                    colorData[colorIndex] = Color.Green;
                            }
                        }
                    }
                }
                float m = (newY - previousY) / (newX - previousX);
                b = previousY;
                for (int x = previousX; x <= newX; x++)
                {
                    if (x > 0)
                    {
                        b = newY - m * newX;
                    }
                    float y = m * x + b;
                    if (Settings.ShowPerlinNoise)
                    {
                        for (int k = -2; k <= 2; k++)
                        {
                            for (int j = -2; j <= 2; j++)
                            {
                                {
                                    int colorIndex = (x + k) + Width * (int)(y + j);
                                    if (colorIndex < colorData.Length && colorIndex >= 0)
                                        colorData[colorIndex] = Color.Red;
                                }
                            }
                        }
                    }
                }
                previousX = newX;
                previousY = newY;
            }
            t2DReturn.SetData(colorData);
            return t2DReturn;
        }

        public void RegenerateTexture()
        {
            Texture = GenerateClouds(20, 2f, 0.1f);
        }

        public void ResetTexture()
        {
            Texture.SetData(EmptyColorData);
        }

        public void Update(GameTime gt)
        {
            int timeDifference = gt.TotalGameTime.Milliseconds - PreviousGameTime.TotalGameTime.Milliseconds;

            PreviousGameTime = gt;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, new Vector2(0, 0), Color.White);
        }
    }
}

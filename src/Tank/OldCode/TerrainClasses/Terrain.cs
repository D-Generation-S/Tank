using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tank.Code
{
    public class Terrain
    {
        private static Terrain instance;

        public static Terrain Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Terrain();
                }
                return instance;
            }
        }

        public Weather TerrainWeather;

        private Texture2D img;
        public Texture2D CurrentMap
        {
            get
            {
                return img;
            }
        }
        public int destructionRes; // how wide is a static pixel

        public int Width
        {
            get
            {
                return img.Width;
            }
        }
        public int Height
        {
            get
            {
                return img.Height;
            }
        }

        private Terrain()
        {
        }

        public bool Initialize(Texture2D pic, int destructionRes)
        {
            this.destructionRes = destructionRes;
            img = pic.ChangePixelColor(new Color(255, 0, 255), Color.Transparent, 0);
            //TerrainWeather = new Weather(Weather.WeatherTypes.Snowfall);
            return true;
        }

        public void Draw(SpriteBatch sb, float x, float y)
        {
            if (img != null)
                sb.Draw(img, new Vector2(x, y), Color.White);
        }

        public void Update()
        {
            img.UpdatePixels();
            if (TerrainWeather != null)
                TerrainWeather.Update();
        }

        public static bool IsPixelSolid(float x, float y)
        {
            if (instance == null || instance.img == null)
                return false;
            bool bReturn = false;
            if (Instance.img.Bounds.Contains(new Vector2(x, y)))
            {
                Color cPixel = GetPixel(x, y);
                bReturn = cPixel != Color.Transparent && cPixel != Color.Green;

            }

            return bReturn;
        }

        public static void AddPixel(Color c, float x, float y, bool Override = true)
        {
            int iPixelIndex = (int)x + Instance.img.Width * (int)y;
            if (Instance.img.Bounds.Contains(new Vector2(x, y)) && Settings.BackgroundPixels.Length > iPixelIndex && GetPixel(x, y) != Color.Black)
            {
                if (!Override && GetPixel(x, y) != Color.Transparent)
                    return;
                Settings.BackgroundPixels[iPixelIndex] = c;
            }
        }

        public static void RemovePixel(float x, float y)
        {
            AddPixel(Color.Transparent, x, y);
        }

        public static Color GetPixel(float x, float y)
        {
            if (instance == null || instance.img == null)
                return Color.Transparent;
            Color Pixel = new Color(255, 0, 255);

            int iPixelIndex = (int)x + Instance.img.Width * (int)y;
            if (Settings.BackgroundPixels.Length > iPixelIndex)
                Pixel = Settings.BackgroundPixels[iPixelIndex];

            return Pixel;
        }

        // Find a normal at a position
        public float[] GetNormal(int x, int y)
        {
            // First find all nearby solid pixels, and create a vector to the average solid pixel from (x,y)
            float avgX = 0;
            float avgY = 0;

            bool[,] bDebug = new bool[7, 7];

            for (int width = -3; width <= 3; width++)
            {
                for (int height = -3; height <= 3; height++)
                {
                    if (IsPixelSolid(x + width, y + height))
                    {
                        bDebug[width + 3, height + 3] = true;
                        avgX -= width;
                        avgY -= height;
                    }
                    else
                        bDebug[width + 3, height + 3] = false;
                }
            }


            float len = (float)Math.Sqrt((avgX * avgX + avgY * avgY));
            float newX = avgX / len;
            float newY = avgY / len;
            return new float[] { newX, newY }; // normalize the vector by dividing by that distance
        }
    }
}

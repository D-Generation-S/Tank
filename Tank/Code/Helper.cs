using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tank.Code.SubClasses;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Tank.Code.GUIClasses;
using Tank.src.DataStructure;

namespace Tank.Code
{
    public static class Helper
    {
        /// <summary>
        /// Replaces all pixels of the specified color with another color
        /// </summary>
        /// <param name="t2D">The Texture2D object to process
        /// <para />
        /// Use this method as a extension for the object.</param>
        /// <param name="ColorToReplace">The Color that needs to be replaces</param>
        /// <param name="ReplacingColor">The Color to replace the previous color(parameter)
        public static Texture2D ChangePixelColor(this Texture2D t2D, Color ColorToReplace, Color ReplacingColor, int tolerance = 0)
        {
            Settings.BackgroundPixels = new Color[t2D.Width * t2D.Height];
            t2D.GetData(Settings.BackgroundPixels);

            for (int x = 0; x < t2D.Width; x++)
            {
                for (int y = 0; y < t2D.Height; y++)
                {
                    int iColorIndex = x + t2D.Width * y;
                    Color pixel = Settings.BackgroundPixels[iColorIndex];
                    if (tolerance > 0)
                    {
                        if (MathHelper.Distance(pixel.R, ColorToReplace.R) <= tolerance ||
                            MathHelper.Distance(pixel.G, ColorToReplace.G) <= tolerance ||
                            MathHelper.Distance(pixel.B, ColorToReplace.B) <= tolerance)
                        {
                            Settings.BackgroundPixels[iColorIndex] = ReplacingColor;
                        }
                        else
                            ApplyTexture(x, y, t2D.Width);
                    }
                    else if (pixel == ColorToReplace)
                        Settings.BackgroundPixels[iColorIndex] = ReplacingColor;
                    else
                        ApplyTexture(x, y, t2D.Width);

                }
            }
            t2D.UpdatePixels();
            return t2D;
        }

        public static Texture2D Combine(this Texture2D BackgroundTexture, Texture2D OverlayTexture, Position Offset)
        {
            Position TextureSize = GetLargeTextureSize(BackgroundTexture, OverlayTexture);
            Texture2D NewTexture = new Texture2D(TankGame.PublicGraphicsDevice, TextureSize.X, TextureSize.Y);


            Color[] colorDataOverlay = new Color[OverlayTexture.Width * OverlayTexture.Height];
            OverlayTexture.GetData<Color>(colorDataOverlay);

            NewTexture = NewTexture.SetTextureData(BackgroundTexture);
            NewTexture = NewTexture.SetTextureData(OverlayTexture);


            return NewTexture;
        }

        private static Texture2D SetTextureData(this Texture2D retTex, Texture2D setTexture)
        {
            int height = retTex.Height;
            int width = retTex.Width;
            Color[] colorData = new Color[width * height];
            retTex.GetData<Color>(colorData);
            Color[] colorDataBackground = new Color[setTexture.Width * setTexture.Height];
            setTexture.GetData<Color>(colorDataBackground);

            Position BackGroundLeftCorner = new Position();
            BackGroundLeftCorner.X = (width / 2) - setTexture.Width / 2;
            BackGroundLeftCorner.Y = (height / 2) - setTexture.Height / 2;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int Index = x + width * y;
                    int StartingIndex = BackGroundLeftCorner.X + width * BackGroundLeftCorner.Y;
                    int InnerTextureEnd = BackGroundLeftCorner.X + setTexture.Width;
                    int EndIndex = InnerTextureEnd + width * (BackGroundLeftCorner.Y + setTexture.Height - 1);
                    if (Index > StartingIndex && x <= InnerTextureEnd && Index < EndIndex)
                    {
                        int innerY = y - BackGroundLeftCorner.Y;
                        int innerX = x - BackGroundLeftCorner.X;
                        colorData[Index] = colorDataBackground[innerX + setTexture.Width * innerY];
                    }
                }
            }
            retTex.SetData<Color>(colorData);

            return retTex;
        }

        public static Position GetLargeTextureSize(params Texture2D[] Textures)
        {
            Position ReturnPosition = new Position();
            ReturnPosition.X = int.MinValue;
            ReturnPosition.Y = int.MinValue;
            foreach (Texture2D CurrentTexture in Textures)
            {
                if (CurrentTexture.Width > ReturnPosition.X)
                    ReturnPosition.X = CurrentTexture.Width;
                if (CurrentTexture.Height > ReturnPosition.Y)
                    ReturnPosition.Y = CurrentTexture.Height;

            }
            return ReturnPosition;
        }

        private static void ApplyTexture(int x, int y, int BaseWidth, bool combineColors = false)
        {
            Color cReturn = Settings.BackgroundPixels[x + BaseWidth * y];

            ColorTextureAssignment imageData = Settings.ColorTextureAssignments.Where(item => item.ColorToReplace == cReturn).FirstOrDefault();

            float R = cReturn.R;
            float G = cReturn.G;
            float B = cReturn.B;
            if (imageData == null)
                return;
            int newX = x % imageData.Width;
            int newY = y % imageData.Height;

            cReturn = imageData.ColorData[newX + imageData.Width * newY];

            if (combineColors)
            {
                R += cReturn.R;
                G += cReturn.G;
                B += cReturn.B;

                R = (R / 2) / 255;
                G = (G / 2) / 255;
                B = (B / 2) / 255;
                cReturn = new Color(R, G, B);
            }
            Settings.BackgroundPixels[x + BaseWidth * y] = cReturn;
        }

        public static void AddColorTextureAssignment(Color c, Texture2D t2D)
        {
            Color[] colorData = new Color[t2D.Width * t2D.Height];
            t2D.GetData(colorData);
            ColorTextureAssignment cta = new ColorTextureAssignment()
            {
                ColorToReplace = c,
                ColorData = colorData,
                Width = t2D.Width,
                Height = t2D.Height
            };
            Settings.ColorTextureAssignments.Add(cta);

        }

        public static bool TakeScreenshot(RenderTarget2D CurrentScreen, bool CreateDirectoryIfNeedet = true)
        {
            string ScreenShotPath = string.Format("{0}\\Screenshot_{1}.png", Settings.ScreenshotFolder, DateTime.Now.ToString("yyyyMMddHmmss"));
            if (!Directory.Exists(Settings.ScreenshotFolder))
            {
                if (CreateDirectoryIfNeedet)
                {
                    Directory.CreateDirectory(Settings.ScreenshotFolder);
                    return TakeScreenshot(CurrentScreen, false);
                }
                return false;
            }

            using (Stream FileStream = File.Create(ScreenShotPath))
            {
                CurrentScreen.SaveAsPng(FileStream, CurrentScreen.Width, CurrentScreen.Height);
            }
            if (File.Exists(ScreenShotPath))
            {
                return true;
            }
            return false;
        }

        public static string[] GetCustomMapsName()
        {
            string[] _files = null;
            if (!Directory.Exists(Settings.CustomMapFolder))
                Directory.CreateDirectory(Settings.CustomMapFolder);

            _files = Directory.GetFiles(Settings.CustomMapFolder);

            return _files;
        }

        public static Texture2D LoadCustomMap(string Name, GraphicsDevice GD)
        {
            Name = $"{Settings.CustomMapFolder + Name}.png";

            Texture2D fileTexture;
            if (!File.Exists(Name))
                return null;
            using (FileStream fileStream = new FileStream(Name, FileMode.Open))
            {
                fileTexture = Texture2D.FromStream(GD, fileStream);
            }

            return fileTexture;
        }

        public static void UpdatePixels(this Texture2D t2D)
        {
            t2D.SetData(Settings.BackgroundPixels);
        }

        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        // Uses Bresenham's line algorithm to efficiently loop between two points, and find the first solid pixel
        // This particular variation always starts from the first point, so collisions don't happen at the wrong end.
        // returns an int array
        //       ||| x = ([0],[1]) point in empty space before collision point
        //       ||| o = ([2],[3]) collision point
        //(end)--||ox------- (start)
        //       |||
        // using http://www.gamedev.net/page/resources/_/reference/programming/sweet-snippets/line-drawing-algorithm-explained-r1275
        public static int[] RayCast(int startX, int startY, int newX, int newY)
        {
            newX = MathHelper.Clamp(newX, 0, 2000);
            newY = MathHelper.Clamp(newY, 0, 2000);
            startX = MathHelper.Clamp(startX, 0, 2000);
            startY = MathHelper.Clamp(startY, 0, 2000);

            int deltax = Math.Abs(newX - startX);        // The difference between the x's
            int deltay = Math.Abs(newY - startY);        // The difference between the y's
            int x = startX;                       // Start x off at the first pixel
            int y = startY;                       // Start y off at the first pixel
            int xinc1, xinc2, yinc1, yinc2;
            int den, num, numadd, numpixels;

            if (newX >= startX)
            {                // The x-values are increasing (Moving right)
                xinc1 = 1;
                xinc2 = 1;
            }
            else
            {                         // The x-values are decreasing (Moving left)
                xinc1 = -1;
                xinc2 = -1;
            }

            if (newY >= startY)
            {                // The y-values are increasing (Falling down)
                yinc1 = 1;
                yinc2 = 1;
            }
            else
            {                         // The y-values are decreasing (Raising up)
                yinc1 = -1;
                yinc2 = -1;
            }

            if (deltax >= deltay) //more right than down | more left than up
            {        // There is at least one x-value for every y-value
                xinc1 = 0;                  // Don't change the x when numerator(Zähler|oben) >= denominator(Nenner|unten)
                yinc2 = 0;                  // Don't change the y for every iteration
                den = deltax;
                num = deltax / 2;
                numadd = deltay;
                numpixels = deltax;         // There are more x-values than y-values
            }
            else //more down than right | more up than left
            {                         // There is at least one y-value for every x-value
                xinc2 = 0;                  // Don't change the x for every iteration
                yinc1 = 0;                  // Don't change the y when numerator >= denominator
                den = deltay;
                num = deltay / 2;
                numadd = deltax;
                numpixels = deltay;         // There are more y-values than x-values
            }
            int prevX = startX;
            int prevY = startY;

            for (int curpixel = 0; curpixel <= numpixels; curpixel++)
            {
                if (Terrain.IsPixelSolid(x, y))
                    return new int[] { prevX, prevY, x, y };
                prevX = x;
                prevY = y;

                num += numadd;              // Increase the numerator by the top of the fraction

                if (num >= den)
                {             // Check if numerator >= denominator
                    num -= den;               // Calculate the new numerator value
                    x += xinc1;               // Change the x as appropriate
                    y += yinc1;               // Change the y as appropriate
                }

                x += xinc2;                 // Change the x as appropriate
                y += yinc2;                 // Change the y as appropriate

                if (Settings.ShowRayCast)
                    Terrain.AddPixel(Color.Green, prevX, prevY);
            }
            return new int[] { };
        }

        private static string GetKeys(this Keys[] keys)
        {
            string ReturnString = "";
            foreach (Keys key in keys)
            {
                ReturnString += key.ToString();
                ReturnString += ", ";
            }
            ReturnString = ReturnString.Remove(ReturnString.Length - 2, 2);
            return ReturnString;
        }

        public static void HorizontalCenterGUIElement(this GUIPrimitiv GUIToCenter)
        {
            GUIToCenter.Position = new Vector2(Settings.MaxWindowSize.Width / 2 - GUIToCenter.Size.X / 2, GUIToCenter.Position.Y);
        }

        public static float GetGroundPosition(float StartX, float StartY)
        {
            float ReturnValue = StartY;
            bool _hitGround = false;
            while (!_hitGround)
            {
                ReturnValue = ReturnValue + 1;
                if (Terrain.IsPixelSolid(StartX, ReturnValue) || ReturnValue >= Terrain.Instance.Height)
                {
                    _hitGround = true;
                    continue;
                }
            }
            return ReturnValue;
        }

        public static Vector2 CalculateHitPosition(float posX, float posY, float vX, float vY)
        {
            Vector2 HitPosition = new Vector2();
            float CurrentPosX = posX;
            float CurrentPosY = posY;
            float NextPosX = 0;
            float NextPosY = 0;
            const int fixedDeltaTime = 16;
            float fixedDeltaTimeSeconds = fixedDeltaTime / 1000.0f;

            bool _waitForHit = true;

            while (_waitForHit)
            {
                float velX = vX;
                float velY = vY;

                velY += 980 * fixedDeltaTimeSeconds;
                vY = velY;
                NextPosX = CurrentPosX + velX * fixedDeltaTimeSeconds;
                if (Terrain.IsPixelSolid(NextPosX, NextPosY) || NextPosX < 0 || NextPosX > Terrain.Instance.Width || NextPosY < -Terrain.Instance.Height)
                {
                    _waitForHit = false;
                    HitPosition = new Vector2(NextPosX, NextPosY);
                    continue;
                }
                else
                {
                    CurrentPosX = NextPosX;
                    CurrentPosY = NextPosY;
                }
            }

            return HitPosition;
        }

        public static List<Vector2> GetBulletTrajectoryLine(float posX, float posY, float vX, float vY)
        {
            List<Vector2> HitPositions = new List<Vector2>();
            float CurrentPosX = posX;
            float CurrentPosY = posY;
            float NextPosX = 0;
            float NextPosY = 0;
            const int fixedDeltaTime = 16;
            float fixedDeltaTimeSeconds = fixedDeltaTime / 1000.0f;

            bool _waitForHit = true;

            while (_waitForHit)
            {
                float velX = vX;
                float velY = vY;

                velY += 980 * fixedDeltaTimeSeconds;
                vY = velY;
                NextPosX = CurrentPosX + velX * fixedDeltaTimeSeconds;
                NextPosY = (CurrentPosY + velY * fixedDeltaTimeSeconds);

                if (Terrain.IsPixelSolid(NextPosX, NextPosY) || NextPosX < 0 || NextPosX > Terrain.Instance.Width || NextPosY < -Terrain.Instance.Height || NextPosY > Terrain.Instance.Height)
                {
                    _waitForHit = false;
                    HitPositions.Add(new Vector2(NextPosX, NextPosY));
                    continue;
                }
                else
                {
                    HitPositions.Add(new Vector2(NextPosX, NextPosY));
                    CurrentPosX = NextPosX;
                    CurrentPosY = NextPosY;
                }
            }

            return HitPositions;
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tank.Code
{
    public class ImageGenerator
    {
        private float[] points;

        private Random Randomizer;
        public GraphicsDevice graphicsDevice;

        private int _seed;
        public int Seed
        {
            get
            {
                return _seed;
            }
            private set
            {
                _seed = value;
            }
        }
        public int MinClimaticZoneWidth = 20;
        public float DirtLevel = 0.3f;
        public int DirtLevelRandomRange = 5;

        public float[] Points
        {
            get

            {
                return points;
            }
        }

        private int ImgWidth;
        private int ImgHeight;
        private float _displace;
        private float _roughness;

        private bool IsAsync;
        private BackgroundWorker GenerateImageWorker;
        private int _workSteps;
        public int WorkSteps
        {
            get
            {
                return _workSteps;
            }
        }
        private int CurrentProgress;

        private bool FillGround;

       //private enum ClimaticZones
       //{
       //    snow,
       //    grass,
       //    dirt,
       //}

        public ImageGenerator(GraphicsDevice gd, int Width, int Height, float displace, float roughness, bool fillGround = false, int seed = int.MinValue)
        {
            graphicsDevice = gd;
            ImgWidth = Width;
            _displace = displace;
            _roughness = roughness;
            FillGround = fillGround;

            ImgHeight = Height;

            if (seed == int.MinValue)
            {
                seed = new Random().Next(DateTime.Now.Millisecond);
            }

            _seed = seed;

            Randomizer = new Random(_seed);
            IsAsync = false;
            CalculateWorkSteps();
        }

        private void CalculateWorkSteps()
        {
            for (int i = 1; i < ImgWidth; i *= 2)
            {
                _workSteps++;
            }
            _workSteps = (int)Math.Pow(2, _workSteps) - 1;
            _workSteps += ImgHeight;

            if (FillGround)
                _workSteps += ImgHeight;
        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            return Randomizer.NextDouble() * (maximum - minimum) + minimum;
        }

        private void GeneratePoints()
        {
            float InternalDisplace = _displace;

            float power = ImgWidth;

            points = new float[(int)power + 1];
            points[0] = (float)(ImgHeight / 2 + (GetRandomNumber(0, 1) * _displace * 2) - _displace);

            points[(int)power] = (float)(ImgHeight / 2 + (GetRandomNumber(0, 1) * _displace * 2) - _displace);

            for (int i = 1; i < power; i *= 2)
            {
                _displace *= _roughness;
                for (float j = (power / i) / 2; j < power; j += power / i)
                {
                    int FirstVal = (int)(j - (power / i) / 2);
                    int SecondVal = (int)(j + (power / i) / 2);
                    points[(int)j] = (Points[FirstVal] + Points[SecondVal]) / 2;
                    points[(int)j] += (float)(GetRandomNumber(0, 1) * _displace * 2) - _displace;
                    if (IsAsync)
                    {
                        CurrentProgress++;
                        GenerateImageWorker.ReportProgress(CurrentProgress);
                    }
                }
                
            }
        }

        public BackgroundWorker GenerateImageAsync()
        {
            CurrentProgress = 0;
            GenerateImageWorker = new BackgroundWorker();
            GenerateImageWorker.DoWork += GenerateImageWorker_DoWork;
            GenerateImageWorker.WorkerReportsProgress = true;
            IsAsync = true;
            GenerateImageWorker.RunWorkerAsync();
            return GenerateImageWorker;
        }

        private void GenerateImageWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = GenerateImage();
        }

        public Texture2D GenerateImage()
        {
            GeneratePoints();
            Texture2D Picture = new Texture2D(graphicsDevice, ImgWidth, ImgHeight);
            Color[] colorData = new Color[ImgWidth * ImgHeight];


            List<int> OldPoints = new List<int>();
            Color CurrenColorToCheck = Color.BlueViolet;
            Color ReplaceColor = CurrenColorToCheck;
            bool FirstZone = true;
            int Counter = 0;
            for (int i = 0; i < Points.Length - 1; i++)
            {
                int y = (int)points[i];
                int Index = i + y * Picture.Width;

                switch (GetClimaticZone(y))
                {
                    case Settings.ClimaticZones.Snow:
                        colorData[Index] = Color.White;
                        break;
                    case Settings.ClimaticZones.Dirt:
                        colorData[Index] = Color.SaddleBrown;
                        break;
                    case Settings.ClimaticZones.Grass:
                        colorData[Index] = Color.Green;
                        break;
                    default:
                        break;
                }
                if (i == 0)
                    CurrenColorToCheck = colorData[Index];
                if (colorData[Index] == CurrenColorToCheck)
                {
                    Counter++;
                    OldPoints.Add(Index);
                }
                else
                {
                    if (Counter < MinClimaticZoneWidth && !FirstZone && OldPoints.Count > 0)
                    {
                        for (int points = 0; points < OldPoints.Count; points++)
                        {
                            colorData[OldPoints[points]] = ReplaceColor; //;
                        }
                        
                    }
                    ReplaceColor = CurrenColorToCheck;
                    CurrenColorToCheck = colorData[Index];
                    OldPoints = new List<int>();
                    OldPoints.Add(Index);
                    FirstZone &= false;
                    Counter = 0;
                }
                if (IsAsync)
                {
                    CurrentProgress++;
                    GenerateImageWorker.ReportProgress(CurrentProgress);
                }
            }

            if (FillGround)
                colorData = FillUpGround(Picture.Width, Picture.Height, colorData, GeHighestPoint());

            Picture.SetData<Color>(colorData);
            IsAsync = false;
            return Picture;
        }

        private int GeHighestPoint()
        {
            int CurrentHighestPoint = int.MinValue;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] > CurrentHighestPoint)
                    CurrentHighestPoint = (int)points[i];
            }
            return CurrentHighestPoint;
        }

        private Settings.ClimaticZones GetClimaticZone(int Height)
        {
            Settings.ClimaticZones ReturnRange = Settings.ClimaticZones.Grass;
            if (Height < ImgHeight * 0.4)
                ReturnRange = Settings.ClimaticZones.Snow;
            else if (Height >= ImgHeight * 0.4 && Height < ImgHeight * 0.6)
                ReturnRange = Settings.ClimaticZones.Dirt;
            return ReturnRange;
        }

        private Color[] FillUpGround(int Width, int Height, Color[] ImgcolorData, int HighestPoint = 200, bool RandomizeLineThickness = false)
        {
            for (int x = 0; x < Width; x++)
            {
                bool StartFill = false;
                int i = 10;
                if (RandomizeLineThickness)
                    i = Randomizer.Next(10, 15);
                Color LineColor = Color.Black;
                int StopDirtLevel = (int)(HighestPoint * DirtLevel);
                int DirtLevelCounter = Randomizer.Next(StopDirtLevel, StopDirtLevel + DirtLevelRandomRange);
                for (int y = 0; y < Height; y++)
                {

                    int index = x + y * Width;
                    if (StartFill)
                    {
                        if (i >= 0)
                        {
                            ImgcolorData[index] = LineColor;
                        }
                        else
                        {
                            if (DirtLevelCounter >= 0)
                            {
                                ImgcolorData[index] = Color.Bisque;
                                DirtLevelCounter--;
                            }
                            else
                            {
                                ImgcolorData[index] = Color.DarkGray;
                            }
                        }

                        i--;
                        continue;
                    }
                    Color CurrentColor = ImgcolorData[index];

                    if (CurrentColor.A != 0 || CurrentColor.G != 0 || CurrentColor.B != 0 || CurrentColor.R != 0)
                    {
                        StartFill = true;
                        LineColor = CurrentColor;
                    }
                }
                if (IsAsync)
                {
                    CurrentProgress++;
                    GenerateImageWorker.ReportProgress(CurrentProgress);
                }
            }
            return ImgcolorData;
        }

    }
}
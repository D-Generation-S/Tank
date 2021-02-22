using Microsoft.Xna.Framework;
using System;
using Tank.Enums;

namespace Tank.Code
{
    public class Weather
    {
        public WeatherTypes CurrentWeather;
        public int WeatherStrengh;

        public Random SpawnRandomGenerator;
        private int MinRangeX, MaxRangeX;
        private int MinRangeY, MaxRangeY;

        public Weather(WeatherTypes Type, int Strengh = 1)
        {
            SpawnRandomGenerator = new Random();
            MinRangeX = 0;
            MaxRangeX = Terrain.Instance.Width;
            MaxRangeY = 0;
            MinRangeY = (int)-(Terrain.Instance.Height * 0.5f);

            CurrentWeather = Type;
        }

        public void Update()
        {
            switch (CurrentWeather)
            {
                case WeatherTypes.ClearSky:
                    break;
                case WeatherTypes.Snowfall:
                    GenerateSnowFall();
                    break;
                default:
                    break;
            }
        }

        private void GenerateSnowFall()
        {
            DynamicPixel NewPixel = new DynamicPixel(Color.White, SpawnRandomGenerator.Next(MinRangeX, MaxRangeX), SpawnRandomGenerator.Next(MinRangeY, MaxRangeY), SpawnRandomGenerator.Next(0, 5), SpawnRandomGenerator.Next(-3, -1), 1, true);
            Renderer.Instance.Add(NewPixel);
            Physics.Instance.Add(NewPixel);
        }
    }
}

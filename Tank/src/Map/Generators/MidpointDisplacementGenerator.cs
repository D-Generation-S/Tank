using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.Entities.Map;
using Tank.src.Interfaces.Randomizer;
using Tank.src.DataStructure;
using Tank.src.Interfaces.MapGenerators;

namespace Tank.Code.MapGenerators.Generatos
{
    class MidpointDisplacementGenerator : IMapGenerator
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly float displace;
        private readonly float roughness;
        private readonly IRandomizer randomizer;

        private Color mapColor;
        public Color MapColor => mapColor;

        public MidpointDisplacementGenerator(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 4)
        {

        }

        public MidpointDisplacementGenerator(GraphicsDevice graphicsDevice, float displace)
            : this(graphicsDevice, displace, 0.4f)
        {

        }

        public MidpointDisplacementGenerator(GraphicsDevice graphicsDevice, float displace, float roughness)
            : this(graphicsDevice, displace, 0.4f, null)
        {
        }

        public MidpointDisplacementGenerator(GraphicsDevice graphicsDevice, float displace, float roughness, IRandomizer randomizer)
        {
            this.graphicsDevice = graphicsDevice;
            this.displace = displace;
            this.roughness = roughness;
            this.randomizer = randomizer;

            mapColor = Color.Black;
        }

        public void SetMapColor(Color newMapColor)
        {
            mapColor = newMapColor;
        }

        public IMap GenerateNewMap(Position size)
        {
            return GenerateNewMap(size, null);
        }

        public IMap GenerateNewMap(Position size, IMapTexturizer mapTexturizer)
        {
            return GenerateNewMap(size, mapTexturizer, int.MinValue);
        }

        public IMap GenerateNewMap(Position size, IMapTexturizer mapTexturizer, int seed)
        {
            seed = seed == int.MinValue ? DateTime.Now.Millisecond : seed;

            Texture2D texture = new Texture2D(graphicsDevice, size.X, size.Y);
            FlattenArray<bool> collisionMap = new FlattenArray<bool>(size.X, size.Y);

            IMap returnMap = new DefaultMap(texture, collisionMap, seed);

            float[] points = GeneratePoints(size, new Random(seed));

            for (int x = 0; x < points.Length - 1; x++)
            {
                returnMap.ChangePixel(x, (int)Math.Round(points[x], 0), mapColor, true);
            }
            returnMap.ApplyChanges();

            FillMap(returnMap);

            if (mapTexturizer != null)
            {
                mapTexturizer.TexturizeMap(returnMap, mapColor);
            }

            return returnMap;
        }

        public void FillMap(IMap map)
        {
            for (int x = 0; x < map.Width; x++)
            {
                bool writeMode = false;
                for (int y = 0; y < map.Height; y++)
                {
                    Color pixel = map.GetPixel(x, y);
                    if (pixel == mapColor)
                    {
                        writeMode = true;
                    }

                    if (writeMode)
                    {
                        map.ChangePixel(x, y, mapColor);
                    }
                }
            }
            map.ApplyChanges();
        }

        private float[] GeneratePoints(Position size, Random internalRandomizer)
        {
            float tempDisplace = displace;
            float power = size.X - 1;

            float[] points = new float[size.X];
            points[0] = size.Y / 2 + (GetRandomNumber(internalRandomizer, 0, 1) * displace * 2) - displace;
            points[(int)power] = (float)(size.Y / 2 + (GetRandomNumber(internalRandomizer, 0, 1) * displace * 2) - displace);

            points[0] = MathHelper.Clamp(points[0], size.Y / 3, size.Y * 2);
            points[(int)power] = MathHelper.Clamp(points[(int)power], size.Y / 3, size.Y * 2);

            for (int i = 1; i < power - 1; i *= 2)
            {
                tempDisplace *= roughness;
                float innerPower = power / i;
                for (float j = innerPower / 2; j < power; j += power / i)
                {
                    int firstValue = (int)(j - innerPower / 2);
                    int secondValue = (int)(j + innerPower / 2);
                    points[(int)j] = (points[firstValue] + points[secondValue]) / 2;
                    points[(int)j] += (float)(GetRandomNumber(internalRandomizer, 0, 1) * tempDisplace * 2) - tempDisplace;
                }
            }

            return points;
        }

        private float GetRandomNumber(Random internalRandomizer, float minimum, float maximum)
        {
            if (randomizer != null)
            {
                return randomizer.GetNewNumber(minimum, maximum);
            }

            return (float)internalRandomizer.NextDouble() * (maximum - minimum) + minimum;
        }

        public async Task<IMap> AsyncGenerateNewMap(Position size)
        {
            IMap returnMap = await Task.Run(() => GenerateNewMap(size, null, int.MinValue));

            return returnMap;
        }

        public async Task<IMap> AsyncGenerateNewMap(Position size, IMapTexturizer mapTexturizer)
        {
            IMap returnMap = await Task.Run(() => GenerateNewMap(size, mapTexturizer, int.MinValue));

            return returnMap;
        }

        public async Task<IMap> AsyncGenerateNewMap(Position size, IMapTexturizer mapTexturizer, int seed)
        {
            IMap returnMap = await Task.Run(() => GenerateNewMap(size, mapTexturizer, seed));

            return returnMap;
        }
    }
}

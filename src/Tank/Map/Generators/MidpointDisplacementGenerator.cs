using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tank.Components;
using Tank.Interfaces.MapGenerators;
using TankEngine.DataStructures;
using TankEngine.Randomizer;

namespace Tank.Map.Generators
{
    /// <summary>
    /// Create a new map using the midpoint displacement algorithm
    /// </summary>
    class MidpointDisplacementGenerator : IMapGenerator
    {
        /// <summary>
        /// The displacement for the map
        /// </summary>
        private readonly float displace;

        /// <summary>
        /// The roughness for the map
        /// </summary>
        private readonly float roughness;

        /// <summary>
        /// The randomizer instance to use
        /// </summary>
        private readonly IRandomizer randomizer;

        /// <summary>
        /// Hash set for all the non solid colors
        /// </summary>
        private readonly HashSet<Color> notSolidColors;

        /// <summary>
        /// The default color to create the map
        /// </summary>
        private Color mapColor;

        /// <summary>
        /// Readonly access to the map color
        /// </summary>
        public Color MapColor => mapColor;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="graphicsDevice">The graphic device to use</param>
        public MidpointDisplacementGenerator()
            : this(225)
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="graphicsDevice">The graphic device to use</param>
        /// <param name="displace">The displace value to use</param>
        public MidpointDisplacementGenerator(float displace)
            : this(displace, 0.4f)
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="graphicsDevice">The graphic device to use</param>
        /// <param name="displace">The displace value to use</param>
        /// <param name="roughness">The roughness to use</param>
        public MidpointDisplacementGenerator(float displace, float roughness)
            : this(displace, roughness, null)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="graphicsDevice">The graphic device to use</param>
        /// <param name="displace">The displace value to use</param>
        /// <param name="roughness">The roughness to use</param>
        /// <param name="randomizer">The randomizer to use</param>
        public MidpointDisplacementGenerator(
            float displace,
            float roughness,
            IRandomizer randomizer
        )
        {
            this.displace = displace;
            this.roughness = roughness;
            this.randomizer = randomizer;
            notSolidColors = new HashSet<Color>();
            notSolidColors.Add(Color.Transparent);

            mapColor = Color.Black;
        }

        /// <summary>
        /// Set the color to use for the map
        /// </summary>
        /// <param name="newMapColor">The new color to use for drawing the map</param>
        public void SetMapColor(Color newMapColor)
        {
            mapColor = newMapColor;
        }

        /// <summary>
        /// Generate a new map with the given size
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <returns>A ready to use instance of the map</returns>
        public MapComponent GenerateNewMap(Point size)
        {
            return GenerateNewMap(size, null);
        }

        /// <summary>
        /// Generate a new map with the given size
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <param name="mapTexturizer">The texturizer to use for the map</param>
        /// <returns>A ready to use instance of the map</returns>
        public MapComponent GenerateNewMap(Point size, IMapTexturizer mapTexturizer)
        {
            return GenerateNewMap(size, mapTexturizer, int.MinValue);
        }

        /// <summary>
        /// Generate a new map with the given size
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <param name="mapTexturizer">The texturizer to use for the map</param>
        /// /// <param name="seed">The seed to use to generate the map</param>
        /// <returns>A ready to use instance of the map</returns>
        public MapComponent GenerateNewMap(Point size, IMapTexturizer mapTexturizer, int seed)
        {
            seed = seed == int.MinValue ? DateTime.Now.Millisecond : seed;
            if (randomizer != null)
            {
                randomizer.Initzialize(seed);
            }

            float[] points = GeneratePoints(size, new Random(seed));
            MapComponent returnMap = new MapComponent()
            {
                ImageData = new FlattenArray<Color>(size.X, size.Y, () => new Color()),
                ChangedImageData = new FlattenArray<Color>(size.X, size.Y, () => new Color()),
                NotSolidColors = notSolidColors,
                HighestPoint = GetHighestPoint(points),
                LowestPoint = getLowestPoint(points),
                Seed = seed,
                Width = size.X,
                Height = size.Y
            };

            for (int x = 0; x < points.Length - 1; x++)
            {
                returnMap.ImageData.SetValue(x, (int)Math.Round(points[x], 0), mapColor);
            }

            FillMap(returnMap);

            if (mapTexturizer != null)
            {
                mapTexturizer.TexturizeMap(returnMap, mapColor);
            }

            return returnMap;
        }

        /// <summary>
        /// Fill the map so that it is solid
        /// </summary>
        /// <param name="map">The map to fill</param>
        private void FillMap(MapComponent map)
        {
            for (int x = 0; x < map.Width; x++)
            {
                bool writeMode = false;
                for (int y = 0; y < map.Height; y++)
                {
                    Color pixel = map.ImageData.GetValue(x, y);
                    if (pixel == mapColor)
                    {
                        writeMode = true;
                    }

                    if (writeMode)
                    {
                        map.ImageData.SetValue(x, y, mapColor);
                    }
                }
            }
        }

        /// <summary>
        /// Get the highest point, this is the smallest in the list since 0 is on top and max is on the bottom
        /// </summary>
        /// <param name="points">All the points</param>
        /// <returns>The highest point</returns>
        private float GetHighestPoint(float[] points)
        {
            return points.Min();
        }

        /// <summary>
        /// Get the lowest point of the curve
        /// </summary>
        /// <param name="points">All the points</param>
        /// <returns>THe lowest point</returns>
        private float getLowestPoint(float[] points)
        {
            return points.Max();
        }

        /// <summary>
        /// Generate all the points for drawing the map
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <param name="internalRandomizer">The internal randomizer to use as fallback</param>
        /// <returns>An array with float values representing the y positions</returns>
        private float[] GeneratePoints(Point size, Random internalRandomizer)
        {
            float tempDisplace = displace;
            float power = size.X - 1;

            float[] points = new float[size.X];
            float randomNumber = GetRandomNumber(internalRandomizer, 0, 1);
            points[0] = size.Y / 2 + randomNumber * displace * 2 - displace;

            randomNumber = GetRandomNumber(internalRandomizer, 0, 1);
            points[(int)power] = size.Y / 2 + randomNumber * displace * 2 - displace;

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
                    randomNumber = GetRandomNumber(internalRandomizer, 0, 1);
                    points[(int)j] += (float)(randomNumber * tempDisplace * 2) - tempDisplace;
                }
            }

            return points;
        }

        /// <summary>
        /// Return a random number
        /// </summary>
        /// <param name="internalRandomizer">The internal randomizer to use as fallback</param>
        /// <param name="minimum">The minimum value to generate</param>
        /// <param name="maximum">The maximum value to generate</param>
        /// <returns>A random float </returns>
        private float GetRandomNumber(Random internalRandomizer, float minimum, float maximum)
        {
            if (randomizer != null)
            {
                return randomizer.GetNewNumber(minimum, maximum);
            }

            return (float)internalRandomizer.NextDouble() * (maximum - minimum) + minimum;
        }

        /// <summary>
        /// Generate the map async
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <returns>An async task which can be watched</returns>
        public async Task<MapComponent> AsyncGenerateNewMap(Point size)
        {
            return await Task.Run(() => GenerateNewMap(size, null, int.MinValue));
        }

        /// <summary>
        /// Generate the map async
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <param name="mapTexturizer">The texturizer to use</param>
        /// <returns>An async task which can be watched</returns>
        public async Task<MapComponent> AsyncGenerateNewMap(Point size, IMapTexturizer mapTexturizer)
        {
            return await Task.Run(() => GenerateNewMap(size, mapTexturizer, int.MinValue));
        }

        /// <summary>
        /// Generate the map async
        /// </summary>
        /// <param name="size">The size of the map</param>
        /// <param name="mapTexturizer">The texturizer to use</param>
        /// <param name="seed">The seed to use</param>
        /// <returns>An async task which can be watched</returns>
        public async Task<MapComponent> AsyncGenerateNewMap(Point size, IMapTexturizer mapTexturizer, int seed)
        {
            return await Task.Run(() => GenerateNewMap(size, mapTexturizer, seed));
        }
    }
}

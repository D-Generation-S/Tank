using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;
using Tank.Interfaces.Entity;

namespace Tank.Interfaces.MapGenerators
{
    interface IMap : IEntity
    {
        /// <summary>
        /// The seed used to generate the map
        /// </summary>
        int Seed { get; }

        /// <summary>
        /// The image to draw the map on the screen
        /// </summary>
        Texture2D Image { get; }

        /// <summary>
        /// The collision data for this map
        /// </summary>
        FlattenArray<bool> CollissionMap { get; }

        /// <summary>
        /// Add or replace a new collideable pixel
        /// </summary>
        /// <param name="position">The position of the new pixel</param>
        /// <param name="color">Thje color of the new pixel</param>
        void AddPixel(Position position, Color color);

        /// <summary>
        /// Add or replace a new pixel on a given location
        /// </summary>
        /// <param name="position">The position to add the pixel</param>
        /// <param name="color">The color of the pixel to add</param>
        /// <param name="collidable">Is this pixel collideable</param>
        void AddPixel(Position position, Color color, bool collidable);

        /// <summary>
        /// Add or replace a new collideable pixel
        /// </summary>
        /// <param name="x">The x position of the pixel</param>
        /// <param name="y">The y position of the pixel</param>
        /// <param name="color">The color of the pixel</param>
        void AddPixel(int x, int y, Color color);

        /// <summary>
        /// Add or replace a new pixel on a given location
        /// </summary>
        /// <param name="x">The x position of the pixel</param>
        /// <param name="y">The y position of the pixel</param>
        /// <param name="color">The color of the pixel to add</param>
        /// <param name="collidable">Is this pixel collideable</param>
        void AddPixel(int x, int y, Color color, bool collidable);

        /// <summary>
        /// Remove a pixel at a given position
        /// </summary>
        /// <param name="position">The position to remove the pixel from</param>
        void RemovePixel(Position position);

        /// <summary>
        /// Remove a pixel at a given position
        /// </summary>
        /// <param name="x">The x position of the pixel</param>
        /// <param name="y">The y position of the pixel</param>
        void RemovePixel(int x, int y);
    }
}

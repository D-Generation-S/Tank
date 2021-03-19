using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tank.DataStructure;

namespace Tank.Interfaces.MapGenerators
{
    /// <summary>
    /// This interface represents a map object
    /// </summary>
    interface IMap
    {
        /// <summary>
        /// The seed used to generate the map
        /// </summary>
        int Seed { get; }

        /// <summary>
        /// The height of the map
        /// </summary>
        int Height { get; }

        /// <summary>
        /// The width of the map
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The image to draw the map on the screen
        /// </summary>
        Texture2D Image { get; }

        /// <summary>
        /// The heighest position on the map
        /// </summary>
        float HighestPosition { get; }

        /// <summary>
        /// The collision data for this map
        /// </summary>
        //FlattenArray<bool> CollissionMap { get; }

        /// <summary>
        /// Add or replace a new collideable pixel
        /// </summary>
        /// <param name="position">The position of the new pixel</param>
        /// <param name="color">Thje color of the new pixel</param>
        void SetPixel(Position position, Color color);

        /// <summary>
        /// Add or replace a new pixel on a given location
        /// </summary>
        /// <param name="position">The position to add the pixel</param>
        /// <param name="color">The color of the pixel to add</param>
        /// <param name="collidable">Is this pixel collideable</param>
        void SetPixel(Position position, Color color, bool collidable);

        /// <summary>
        /// Add or replace a new collideable pixel
        /// </summary>
        /// <param name="x">The x position of the pixel</param>
        /// <param name="y">The y position of the pixel</param>
        /// <param name="color">The color of the pixel</param>
        void SetPixel(int x, int y, Color color);


        /// <summary>
        /// Add or replace a new pixel on a given location
        /// </summary>
        /// <param name="x">The x position of the pixel</param>
        /// <param name="y">The y position of the pixel</param>
        /// <param name="color">The color of the pixel to add</param>
        /// <param name="collidable">Is this pixel collideable</param>
        void SetPixel(int x, int y, Color color, bool collidable);

        /// <summary>
        /// This will change a pixel in cache for later applying
        /// </summary>
        /// <param name="position">The position of the pixel to change</param>
        /// <param name="color">The new color of the pixel</param>
        void ChangePixel(Position position, Color color);

        /// <summary>
        /// This will change a pixel in cache for later applying
        /// </summary>
        /// <param name="position">The position of the pixel to change</param>
        /// <param name="color">The new color of the pixel</param>
        /// <param name="collidable">Is this new pixel collideable</param>
        void ChangePixel(Position position, Color color, bool collidable);

        /// <summary>
        /// This will change a pixel in cache for later applying
        /// </summary>
        /// <param name="x">The x position of the pixel to change</param>
        /// <param name="y">The y position of the pixel to change</param>
        /// <param name="color">The new color of the pixel</param>
        void ChangePixel(int x, int y, Color color);

        /// <summary>
        /// This will change a pixel in cache for later applying
        /// </summary>
        /// <param name="x">The x position of the pixel to change</param>
        /// <param name="y">The y position of the pixel to change</param>
        /// <param name="color">The new color of the pixel</param>
        /// <param name="collidable">Is this new pixel collideable</param>
        void ChangePixel(int x, int y, Color color, bool collidable);

        /// <summary>
        /// This will revert all the changes done by the change pixel method
        /// </summary>
        void RevertChanges();

        /// <summary>
        /// This will apply all the changes done by the change pixel method
        /// </summary>
        void ApplyChanges();

        /// <summary>
        /// Get the pixel at a given position
        /// </summary>
        /// <param name="position">The position to get the pixel from</param>
        Color GetPixel(Position position);

        /// <summary>
        /// Get the pixel at a given postion
        /// </summary>
        /// <param name="x">The x position of the pixel to get</param>
        /// <param name="y">The y position of the pixel to get</param>
        /// <returns></returns>
        Color GetPixel(int x, int y);

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

        /// <summary>
        /// Check if the given pixel is solid
        /// </summary>
        /// <param name="vector">The position to check</param>
        /// <returns>True if solid</returns>
        bool IsPixelSolid(Vector2 vector);

        /// <summary>
        /// Check if the given pixel is solid
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <returns>True if solid</returns>
        bool IsPixelSolid(Position position);

        /// <summary>
        /// Check if the given pixel is solid
        /// </summary>
        /// <param name="x">The x component of the position</param>
        /// <param name="y">The y component of the position</param>
        /// <returns>True if solid</returns>
        bool IsPixelSolid(int x, int y);

        /// <summary>
        /// Is a given point still on the map
        /// </summary>
        /// <param name="vector">The position to check</param>
        /// <returns>True if on map</returns>
        bool IsPointOnMap(Vector2 vector);

        /// <summary>
        /// Is a given point still on the map
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <returns>True if on map</returns>
        bool IsPointOnMap(Position position);

        /// <summary>
        /// Is a given point still on the map
        /// </summary>
        /// <param name="x">The x component of the position</param>
        /// <param name="y">The y component of the position</param>
        /// <returns></returns>
        bool IsPointOnMap(int x, int y);
    }
}

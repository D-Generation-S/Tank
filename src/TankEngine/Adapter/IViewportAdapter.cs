using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TankEngine.Adapter
{
    /// <summary>
    /// Interface for a viewport adapter
    /// </summary>
    public interface IViewportAdapter : IDisposable
    {
        /// <summary>
        /// The virtual width
        /// </summary>
        int VirtualWidth { get; }

        /// <summary>
        /// The virtual height
        /// </summary>
        int VirtualHeight { get; }

        /// <summary>
        /// The center of the viewport
        /// </summary>
        Point Center { get; }

        /// <summary>
        /// The bounding box of the viewport
        /// </summary>
        Rectangle BoundingRectangle { get; }

        /// <summary>
        /// The real viewport
        /// </summary>
        Viewport Viewport { get; }

        /// <summary>
        /// The virtual viewport
        /// </summary>
        Viewport VirtualViewport { get; }

        /// <summary>
        /// Get the scale matrix
        /// </summary>
        /// <returns>The scaled matrix</returns>
        Matrix GetScaleMatrix();

        /// <summary>
        /// Reset the viewport
        /// </summary>
        void Reset();

        /// <summary>
        /// Get a point on the screent
        /// </summary>
        /// <param name="point">The point to transfere</param>
        /// <returns>The real point</returns>
        Point GetPointOnScreen(Point point);

        /// <summary>
        /// Get a point on the screent
        /// </summary>
        /// <param name="point">The point to transfere</param>
        /// <returns>The real point</returns>
        Point GetPointOnScreen(Vector2 point);

        /// <summary>
        /// Get a point on the screent
        /// </summary>
        /// <param name="point">The point to transfere</param>
        /// <returns>The real point as vector</returns>
        Vector2 GetVectorPointOnScreen(Point point);

        /// <summary>
        /// Get a point on the screent
        /// </summary>
        /// <param name="point">The point to transfere</param>
        /// <returns>The real point as vector</returns>
        Vector2 GetVectorPointOnScreen(Vector2 point);

    }
}

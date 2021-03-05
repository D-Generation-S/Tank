using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Adapter
{
    public interface IViewportAdapter : IDisposable
    {
        int VirtualWidth { get; }
        int VirtualHeight { get; }
        Point Center { get; }
        Rectangle BoundingRectangle { get; }
        Viewport Viewport { get; }
        Viewport VirtualViewport { get; }

        Matrix GetScaleMatrix();
        void Reset();

        Point GetPointOnScreen(Point point);
        Point GetPointOnScreen(Vector2 point);
        Vector2 GetVectorPointOnScreen(Point point);
        Vector2 GetVectorPointOnScreen(Vector2 point);

    }
}

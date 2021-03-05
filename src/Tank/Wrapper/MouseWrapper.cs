using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.Adapter;

namespace Tank.Wrapper
{
    class MouseWrapper
    {
        private readonly IViewportAdapter viewportAdapter;

        public MouseWrapper(IViewportAdapter viewportAdapter)
        {
            this.viewportAdapter = viewportAdapter;
        }

        public Point GetMousePosition()
        {
            return GetPosition(Mouse.GetState().Position);
        }

        public Vector2 GetMouseVectorPosition()
        {
            return GetVectorPosition(Mouse.GetState().Position);
        }

        public Point GetPosition(Point position)
        {
            return viewportAdapter == null ? position : viewportAdapter.GetPointOnScreen(position);
        }

        public Vector2 GetVectorPosition(Point position)
        {
            return GetPosition(position).ToVector2();
        }
    }
}

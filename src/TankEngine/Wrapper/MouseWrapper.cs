using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TankEngine.Adapter;

namespace TankEngine.Wrapper
{
    /// <summary>
    /// Wrapepr for the mouse class
    /// </summary>
    public class MouseWrapper
    {
        /// <summary>
        /// The viewort adapter to use
        /// </summary>
        private readonly IViewportAdapter viewportAdapter;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="viewportAdapter">The viewport adapter to use</param>
        public MouseWrapper(IViewportAdapter viewportAdapter)
        {
            this.viewportAdapter = viewportAdapter;
        }

        /// <summary>
        /// Get the mouse position with the current viewport
        /// </summary>
        /// <returns>The real mouse position</returns>
        public Point GetMousePosition()
        {
            return GetPosition(Mouse.GetState().Position);
        }

        /// <summary>
        /// Get thew mouse position with the current viewport as Vector2
        /// </summary>
        /// <returns>The real mouse position</returns>
        public Vector2 GetMouseVectorPosition()
        {
            return GetVectorPosition(Mouse.GetState().Position);
        }

        /// <summary>
        /// Get the position of a given point
        /// </summary>
        /// <param name="position">The position to tranfere</param>
        /// <returns>The real position</returns>
        public Point GetPosition(Point position)
        {
            return viewportAdapter == null ? position : viewportAdapter.GetPointOnScreen(position);
        }

        /// <summary>
        /// Get the position of a given point as Vector2
        /// </summary>
        /// <param name="position">The position to tranfere</param>
        /// <returns>The real position</returns>
        public Vector2 GetVectorPosition(Point position)
        {
            return GetPosition(position).ToVector2();
        }
    }
}

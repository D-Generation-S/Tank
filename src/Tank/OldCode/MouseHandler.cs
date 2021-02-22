using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Tank.Code.Screenmanager.ViewPortAdapters;

namespace Tank.Code
{
    public static class MouseHandler
    {
        public static Vector2 Position
        {
            get; set;
        }
        public static ButtonState LeftButton
        {
            get; set;
        }

        public static void Update(MouseState mouseState, BoxingViewportAdapter adapter)
        {
            Position = adapter.PointToScreen(mouseState.Position).ToVector2();
            LeftButton = mouseState.LeftButton;
        }
    }
}

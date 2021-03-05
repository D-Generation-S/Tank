using Microsoft.Xna.Framework;
using Tank.GameStates;
using Tank.Wrapper;

namespace Tank.Gui
{
    interface IGuiElement : GameStates.IUpdateable, GameStates.IDrawable
    {
        void SetMouseWrapper(MouseWrapper mouseWrapper);
        string Name { get; set; }
        Vector2 Size { get; }
        public Vector2 Position { get; }
        void SetPosition(Vector2 position);
    }
}

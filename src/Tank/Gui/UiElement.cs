using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tank.DataStructure.Spritesheet;
using Tank.Wrapper;

namespace Tank.Gui
{
    class UiElement : IGuiElement
    {
        public Vector2 Position { get; protected set; }
        public Vector2 Size { get; protected set; }
        protected readonly int width;
        protected MouseWrapper mouseWrapper;
        public string Name { get; set; }

        public UiElement(Vector2 position, int width)
        {
            Position = position;
            Size = Vector2.Zero;
            this.width = width;
            Name = string.Empty;
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public virtual void SetMouseWrapper(MouseWrapper mouseWrapper)
        {
            this.mouseWrapper = mouseWrapper;
        }

        protected Point GetMousePosition()
        {
            Point position = Mouse.GetState().Position;
            return mouseWrapper == null ? position : mouseWrapper.GetPosition(position);
        }
    }
}

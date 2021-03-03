using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    class UiElement : IGuiElement
    {
        public Vector2 Position { get; protected set; }
        public Vector2 Size { get; protected set; }
        protected readonly int width;
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
    }
}

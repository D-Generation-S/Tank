using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TankEngine.Wrapper;

namespace TankEngine.Gui
{
    /// <summary>
    /// A simple UI element
    /// </summary>
    public class UiElement : IGuiElement
    {
        /// <inheritdoc/>
        public Vector2 Position { get; protected set; }

        /// <inheritdoc/>
        public Vector2 Size { get; protected set; }

        /// <summary>
        /// The widht of the elment
        /// </summary>
        protected readonly int width;

        /// <summary>
        /// The mouse wrapper to use
        /// </summary>
        protected MouseWrapper mouseWrapper;

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position to use</param>
        /// <param name="width">The width to apply</param>
        public UiElement(Vector2 position, int width)
        {
            Position = position;
            Size = Vector2.Zero;
            this.width = width;
            Name = string.Empty;
        }

        /// <inheritdoc/>
        public virtual void Draw(GameTime gameTime)
        {
        }

        /// <inheritdoc/>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <inheritdoc/>
        public virtual void SetPosition(Vector2 position)
        {
            Position = position;
        }

        /// <inheritdoc/>
        public virtual void SetMouseWrapper(MouseWrapper mouseWrapper)
        {
            this.mouseWrapper = mouseWrapper;
        }

        /// <inheritdoc/>
        protected Point GetMousePosition()
        {
            Point position = Mouse.GetState().Position;
            return mouseWrapper == null ? position : mouseWrapper.GetPosition(position);
        }
    }
}

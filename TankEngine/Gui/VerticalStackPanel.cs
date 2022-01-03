using Microsoft.Xna.Framework;
using TankEngine.Adapter;
using TankEngine.Wrapper;

namespace Tank.Gui
{
    /// <summary>
    /// A panel which stacks ui elements on top of each other
    /// </summary>
    public class VerticalStackPanel : Panel
    {
        /// <summary>
        /// Center of the stack panel
        /// </summary>
        private readonly float center;

        /// <summary>
        /// The viewport adapter used by the game
        /// </summary>
        private readonly IViewportAdapter viewport;

        /// <summary>
        /// Space between the elements
        /// </summary>
        private readonly int elementSpace;

        /// <summary>
        /// Should center the elements on the vertical center
        /// </summary>
        private readonly bool centerVertical;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="width">The width of the class</param>
        /// <param name="elementSpace">The space between the elements</param>
        public VerticalStackPanel(IViewportAdapter viewport, int width, int elementSpace)
    : this(viewport, Vector2.Zero, width, elementSpace)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position of the panel</param>
        /// <param name="width">The width of the class</param>
        /// <param name="elementSpace">The space between the elements</param>
        public VerticalStackPanel(IViewportAdapter viewport, Vector2 position, int width, int elementSpace)
            : this(viewport, position, width, elementSpace, false)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position of the panel</param>
        /// <param name="width">The width of the class</param>
        /// <param name="elementSpace">The space between the elements</param>
        /// <param name="centerVertical">Should place elements on vertical middle</param>
        public VerticalStackPanel(IViewportAdapter viewport, Vector2 position, int width, int elementSpace, bool centerVertical) : base(position, width)
        {
            center = width / 2;
            this.viewport = viewport;
            this.elementSpace = elementSpace;
            this.centerVertical = centerVertical;
        }

        /// <inheritdoc/>
        public override void SetMouseWrapper(MouseWrapper mouseWrapper)
        {
            base.SetMouseWrapper(mouseWrapper);
            foreach (IGuiElement guiElement in Container)
            {
                guiElement.SetMouseWrapper(mouseWrapper);
            }
        }

        /// <inheritdoc/>
        public override void AddElement(IGuiElement elementToAdd)
        {
            elementToAdd.SetMouseWrapper(mouseWrapper);
            if (centerVertical)
            {
                base.AddElement(elementToAdd);

                Vector2 center = Vector2.UnitY * viewport.VirtualViewport.Height / 2;
                float totalHeight = 0;
                foreach (IGuiElement element in Container)
                {
                    totalHeight += element.Size.Y;
                }
                if (Container.Count > 0)
                {
                    totalHeight += elementSpace * Container.Count - 1;
                }

                Vector2 firstPosition = center;
                firstPosition.Y -= totalHeight / 2;
                Vector2 lastBottom = firstPosition;
                for (int i = 0; i < Container.Count; i++)
                {
                    IGuiElement currentElement = Container[i];
                    currentElement.SetPosition(lastBottom);
                    lastBottom.Y += currentElement.Size.Y + elementSpace;
                }
                return;
            }

            Vector2 position = Position;
            position += Vector2.UnitX * center;
            position.X -= elementToAdd.Size.X / 2;

            if (Container.Count != 0)
            {
                IGuiElement previousElement = Container[Container.Count - 1];
                position.Y = previousElement.Position.Y;
                position.Y += previousElement.Size.Y + elementSpace;
            }

            base.AddElement(elementToAdd);
            elementToAdd.SetPosition(position);
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Linq;
using TankEngine.Adapter;
using TankEngine.Wrapper;

namespace TankEngine.Gui
{
    /// <summary>
    /// A panel which stacks ui elements on top of each other
    /// </summary>
    public class VerticalStackPanel : Panel
    {
        /// <summary>
        /// Center of the stack panel
        /// </summary>
        private readonly float horizontalCenter;

        /// <summary>
        /// The vertical center of the stack panel
        /// </summary>
        private readonly float verticalCenter;

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
        /// Center the element on the horizonal
        /// </summary>
        private readonly bool centerHorizontal;

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
        public VerticalStackPanel(IViewportAdapter viewport, Vector2 position, int width, int elementSpace, bool centerVertical)
            : this(viewport, position, width, elementSpace, centerVertical, false)
        {

        }


        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position of the panel</param>
        /// <param name="width">The width of the class</param>
        /// <param name="elementSpace">The space between the elements</param>
        /// <param name="centerVertical">Should place elements on vertical middle</param>
        public VerticalStackPanel(IViewportAdapter viewport, Vector2 position, int width, int elementSpace, bool centerVertical, bool centerHorizontal)
            : base(position, width)
        {
            horizontalCenter = width / 2;
            verticalCenter = viewport.VirtualHeight / 2;
            this.viewport = viewport;
            this.elementSpace = elementSpace;
            this.centerVertical = centerVertical;
            this.centerHorizontal = centerHorizontal;
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
                AddElementAtCenter(elementToAdd);
                return;
            }
            AddElementToLeft(elementToAdd);
        }

        /// <summary>
        /// Center all the elements horizontally
        /// </summary>
        private void CenterHorizontal()
        {
            if (!centerHorizontal)
            {
                return;
            }
            foreach (IGuiElement guiElement in Container)
            {
                Vector2 position = guiElement.Position;
                position.X = horizontalCenter - guiElement.Size.X;
                guiElement.SetPosition(position);
            }
        }

        /// <summary>
        /// Add a element vertically centered
        /// </summary>
        /// <param name="elementToAdd">The element to add</param>
        private void AddElementAtCenter(IGuiElement elementToAdd)
        {
            base.AddElement(elementToAdd);

            int containerCount = Math.Max(1, Container.Count - 1);
            float totalHeight = Container.Sum(element => element.Size.Y) + elementSpace * containerCount;
            Vector2 startPosition = Vector2.UnitY * (verticalCenter - totalHeight / 2);
            for (int i = 0; i < Container.Count; i++)
            {
                float usedUpSpace = Container.Take(i).Sum((element => element.Size.Y));
                IGuiElement currentElement = Container[i];

                Vector2 position = startPosition;
                position.Y += (elementSpace * i) + usedUpSpace;
                currentElement.SetPosition(position);
            }
            CenterHorizontal();
        }

        /// <summary>
        /// Add a element to the panel on the left side
        /// </summary>
        /// <param name="elementToAdd">The element to add</param>
        private void AddElementToLeft(IGuiElement elementToAdd)
        {
            Vector2 position = Position;
            position += Vector2.UnitX * horizontalCenter;
            position.X -= elementToAdd.Size.X / 2;

            if (Container.Count != 0)
            {
                IGuiElement previousElement = Container[Container.Count - 1];
                position.Y = previousElement.Position.Y;
                position.Y += previousElement.Size.Y + elementSpace;
            }

            base.AddElement(elementToAdd);
            elementToAdd.SetPosition(position);
            CenterHorizontal();
        }
    }
}

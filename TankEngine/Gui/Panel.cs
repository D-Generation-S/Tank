using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Tank.Gui
{
    /// <summary>
    /// Base class for all panel type ui elements
    /// </summary>
    public class Panel : UiElement
    {
        /// <summary>
        /// A list with all the sub elements
        /// </summary>
        public List<IGuiElement> Container { get; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position to use</param>
        /// <param name="width">The width of the element</param>
        public Panel(Vector2 position, int width) : base(position, width)
        {
            Container = new List<IGuiElement>();
        }

        /// <summary>
        /// Add a new element to the container
        /// </summary>
        /// <param name="elementToAdd">The element to add</param>
        public virtual void AddElement(IGuiElement elementToAdd)
        {
            if (Container.Contains(elementToAdd))
            {
                return;
            }
            Container.Add(elementToAdd);
        }

        /// <summary>
        /// Return a gui element by it's name
        /// </summary>
        /// <param name="name">The name of the gui element</param>
        /// <returns>The gui element or null</returns>
        public IGuiElement GetElementByName(string name)
        {
            return Container.Find(item => item.Name == name);
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime)
        {
            foreach (IGuiElement guiElement in Container)
            {
                guiElement.Draw(gameTime);
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            foreach (IGuiElement guiElement in Container)
            {
                guiElement.Update(gameTime);
            }
        }
    }
}

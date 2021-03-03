using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    class Panel : UiElement
    {
        public List<IGuiElement> Container { get; }

        public Panel(Vector2 position, int width) : base(position, width)
        {
            Container = new List<IGuiElement>();
        }

        public virtual void AddElement(IGuiElement elementToAdd)
        {
            if (Container.Contains(elementToAdd))
            {
                return;
            }
            Container.Add(elementToAdd);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (IGuiElement guiElement in Container)
            {
                guiElement.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (IGuiElement guiElement in Container)
            {
                guiElement.Update(gameTime);
            }
        }
    }
}

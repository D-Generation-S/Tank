using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    class VerticalStackPanel : Panel
    {
        private readonly float center;
        private readonly int elementSpace;
        private readonly bool centerVertical;

        public VerticalStackPanel(Vector2 position, int width, int elementSpace)
            :this(position, width, elementSpace, false)
        {
        }

        public VerticalStackPanel(Vector2 position, int width, int elementSpace, bool centerVertical) : base(position, width)
        {
            center = width / 2;
            this.elementSpace = elementSpace;
            this.centerVertical = centerVertical;
        }

        public override void AddElement(IGuiElement elementToAdd)
        {
            if (centerVertical)
            {
                base.AddElement(elementToAdd);

                Vector2 center = Vector2.UnitY * TankGame.PublicGraphicsDevice.Viewport.Height / 2;
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

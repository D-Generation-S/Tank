using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    class VisibleUiElement : UiElement
    {
        protected readonly SpriteSheet textureToShow;
        protected readonly SpriteBatch spriteBatch;

        public VisibleUiElement(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spriteBatch) : base(position, width)
        {
            this.textureToShow = textureToShow;
            this.spriteBatch = spriteBatch;
        }

        protected Vector2 GetTextLenght(string text, SpriteFont font)
        {
            return font.MeasureString(text);
        }
    }
}

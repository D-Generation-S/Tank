using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    class UiElement : IGuiElement
    {
        protected readonly Vector2 position;
        protected readonly int width;
        protected readonly SpriteSheet textureToShow;
        protected readonly SpriteBatch spriteBatch;

        public UiElement(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spriteBatch)
        {
            this.position = position;
            this.width = width;
            this.textureToShow = textureToShow;
            this.spriteBatch = spriteBatch;
        }

        public void Draw(GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        protected Vector2 GetTextLenght(string text, SpriteFont font)
        {
            return font.MeasureString(text);
        }
    }
}

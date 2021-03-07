using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Tank.DataStructure.Spritesheet;
using Tank.Gui;

namespace Tank.Factories.Gui
{
    /// <summary>
    /// This class will generate selection boxes
    /// </summary>
    class SelectionBoxFactory : AbstractGuiFactory<SelectBox>
    {
        /// <inheritdoc/>
        public SelectionBoxFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch) : base(font, spriteSheet, spriteBatch)
        {
        }

        /// <inheritdoc/>
        public SelectionBoxFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width) : base(font, spriteSheet, spriteBatch, width)
        {
        }

        /// <inheritdoc/>
        public SelectionBoxFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width, Vector2 position) : base(font, spriteSheet, spriteBatch, width, position)
        {
        }

        /// <inheritdoc/>
        public SelectionBoxFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound) : base(font, spriteSheet, spriteBatch, width, position, clickSound)
        {
        }

        /// <inheritdoc/>
        public SelectionBoxFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound, SoundEffect hoverSound) : base(font, spriteSheet, spriteBatch, width, position, clickSound, hoverSound)
        {
        }

        /// <inheritdoc/>
        public override SelectBox GetNewObject()
        {
            SelectBox returnBox = new SelectBox(Vector2.Zero, 100, spriteSheet, spriteBatch);
            returnBox.SetFont(font);
            returnBox.SetClickEffect(clickSound);
            returnBox.SetHoverEffect(hoverSound);

            return returnBox;
        }
    }
}

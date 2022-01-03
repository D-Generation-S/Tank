using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Tank.DataStructure.Spritesheet;
using Tank.Gui;

namespace Tank.Factories.Gui
{
    /// <summary>
    /// A factory to create a simple button
    /// </summary>
    public class ButtonFactory : AbstractGuiFactory<Button>
    {
        /// <inheritdoc/>
        public ButtonFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch) : base(font, spriteSheet, spriteBatch)
        {
        }

        /// <inheritdoc/>
        public ButtonFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width) : base(font, spriteSheet, spriteBatch, width)
        {
        }

        /// <inheritdoc/>
        public ButtonFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width, Vector2 position) : base(font, spriteSheet, spriteBatch, width, position)
        {
        }

        /// <inheritdoc/>
        public ButtonFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound) : base(font, spriteSheet, spriteBatch, width, position, clickSound)
        {
        }

        /// <inheritdoc/>
        public ButtonFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound, SoundEffect hoverSound) : base(font, spriteSheet, spriteBatch, width, position, clickSound, hoverSound)
        {
        }

        /// <inheritdoc/>
        public override Button GetNewObject()
        {
            Button returnButton = new Button(position, width, spriteSheet, spriteBatch);
            returnButton.SetFont(font);
            returnButton.SetClickEffect(clickSound);
            returnButton.SetHoverEffect(hoverSound);
            return returnButton;
        }
    }
}

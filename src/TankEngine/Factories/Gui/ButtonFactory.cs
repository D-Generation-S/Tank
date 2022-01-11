using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TankEngine.DataStructures.Spritesheet;
using TankEngine.Gui;

namespace TankEngine.Factories.Gui
{
    /// <summary>
    /// A factory to create a simple button
    /// </summary>
    public class ButtonFactory : AbstractGuiFactory<Button>
    {
        /// <inheritdoc/>
        public ButtonFactory(SpriteFont font, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, int width)
            : base(font, spritesheetTexture, spriteBatch, width)
        {
        }

        /// <inheritdoc/>
        public ButtonFactory(SpriteFont font, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, int width, Vector2 position)
            : base(font, spritesheetTexture, spriteBatch, width, position)
        {
        }

        /// <inheritdoc/>
        public ButtonFactory(SpriteFont font, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound)
            : base(font, spritesheetTexture, spriteBatch, width, position, clickSound)
        {
        }

        /// <inheritdoc/>
        public ButtonFactory(SpriteFont font, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound, SoundEffect hoverSound)
            : base(font, spritesheetTexture, spriteBatch, width, position, clickSound, hoverSound)
        {
        }

        /// <inheritdoc/>
        public override Button GetNewObject()
        {
            Button returnButton = new Button(position, width, spritesheetTexture, spriteBatch);
            returnButton.SetFont(font);
            returnButton.SetClickEffect(clickSound);
            returnButton.SetHoverEffect(hoverSound);
            return returnButton;
        }
    }
}

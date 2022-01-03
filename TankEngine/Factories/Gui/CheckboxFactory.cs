using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TankEngine.DataStructures.Spritesheet;
using TankEngine.Gui;

namespace TankEngine.Factories.Gui
{
    public class CheckboxFactory : AbstractGuiFactory<Checkbox>
    {
        /// <inheritdoc/>
        public CheckboxFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch) : base(font, spriteSheet, spriteBatch)
        {
        }

        /// <inheritdoc/>
        public CheckboxFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width) : base(font, spriteSheet, spriteBatch, width)
        {
        }

        /// <inheritdoc/>
        public CheckboxFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width, Vector2 position) : base(font, spriteSheet, spriteBatch, width, position)
        {
        }

        /// <inheritdoc/>
        public CheckboxFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound) : base(font, spriteSheet, spriteBatch, width, position, clickSound)
        {
        }

        /// <inheritdoc/>
        public CheckboxFactory(SpriteFont font, SpriteSheet spriteSheet, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound, SoundEffect hoverSound) : base(font, spriteSheet, spriteBatch, width, position, clickSound, hoverSound)
        {
        }

        /// <inheritdoc/>
        public override Checkbox GetNewObject()
        {
            Checkbox returnCheckbox = new Checkbox(position, width, spriteSheet, spriteBatch);
            returnCheckbox.SetFont(font);
            returnCheckbox.SetClickEffect(clickSound);
            returnCheckbox.SetHoverEffect(hoverSound);
            returnCheckbox.SetRenderOffset(Vector2.UnitX * 11);

            return returnCheckbox;
        }
    }
}

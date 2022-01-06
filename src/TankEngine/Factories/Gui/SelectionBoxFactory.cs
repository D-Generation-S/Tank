using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TankEngine.DataStructures.Spritesheet;
using TankEngine.Gui;

namespace TankEngine.Factories.Gui
{
    /// <summary>
    /// This class will generate selection boxes
    /// </summary>
    public class SelectionBoxFactory : AbstractGuiFactory<SelectBox>
    {

        /// <inheritdoc/>
        public SelectionBoxFactory(SpriteFont font, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, int width)
            : base(font, spritesheetTexture, spriteBatch, width)
        {
        }

        /// <inheritdoc/>
        public SelectionBoxFactory(SpriteFont font, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, int width, Vector2 position)
            : base(font, spritesheetTexture, spriteBatch, width, position)
        {
        }

        /// <inheritdoc/>
        public SelectionBoxFactory(SpriteFont font, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound)
            : base(font, spritesheetTexture, spriteBatch, width, position, clickSound)
        {
        }

        /// <inheritdoc/>
        public SelectionBoxFactory(SpriteFont font, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound, SoundEffect hoverSound)
            : base(font, spritesheetTexture, spriteBatch, width, position, clickSound, hoverSound)
        {
        }

        /// <inheritdoc/>
        public override SelectBox GetNewObject()
        {
            SelectBox returnBox = new SelectBox(Vector2.Zero, width, spritesheetTexture, spriteBatch);
            returnBox.SetTextOffset(10);
            returnBox.SetFont(font);
            returnBox.SetClickEffect(clickSound);
            returnBox.SetHoverEffect(hoverSound);

            return returnBox;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TankEngine.DataStructures.Spritesheet;

namespace TankEngine.Factories.Gui
{
    /// <summary>
    /// Abstract class to create ui elements
    /// </summary>
    /// <typeparam name="T">The type of the factory</typeparam>
    public abstract class AbstractGuiFactory<T> : IFactory<T>
    {
        /// <summary>
        /// The spriteshett to use
        /// </summary>
        protected readonly SpritesheetTexture spritesheetTexture;

        /// <summary>
        /// The font to use
        /// </summary>
        protected readonly SpriteFont font;

        /// <summary>
        /// The spritebatch to use
        /// </summary>
        protected readonly SpriteBatch spriteBatch;

        /// <summary>
        /// The position to initial set the ui element
        /// </summary>
        protected readonly Vector2 position;

        /// <summary>
        /// The width of the ui element
        /// </summary>
        protected readonly int width;

        /// <summary>
        /// The sound for clicking the ui element
        /// </summary>
        protected readonly SoundEffect clickSound;

        /// <summary>
        /// The sound if you hover over the ui element
        /// </summary>
        protected readonly SoundEffect hoverSound;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="font">The font to use</param>
        /// <param name="spriteSheet">The spritesheet to use</param>
        /// <param name="spriteBatch">The spritebatch to use</param>
        /// <param name="width">The width of the element</param>
        public AbstractGuiFactory(SpriteFont font, SpritesheetTexture spriteSheetTexture, SpriteBatch spriteBatch, int width)
            : this(font, spriteSheetTexture, spriteBatch, width, Vector2.Zero)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="font">The font to use</param>
        /// <param name="spriteSheet">The spritesheet to use</param>
        /// <param name="spriteBatch">The spritebatch to use</param>
        /// <param name="width">The width of the element</param>
        /// <param name="position">The position of the element</param>
        public AbstractGuiFactory(SpriteFont font, SpritesheetTexture spriteSheetTexture, SpriteBatch spriteBatch, int width, Vector2 position)
            : this(font, spriteSheetTexture, spriteBatch, width, position, null)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="font">The font to use</param>
        /// <param name="spriteSheet">The spritesheet to use</param>
        /// <param name="spriteBatch">The spritebatch to use</param>
        /// <param name="width">The width of the element</param>
        /// <param name="position">The position of the element</param>
        /// <param name="clickSound">The sound if element got clicked</param>
        public AbstractGuiFactory(SpriteFont font, SpritesheetTexture spriteSheetTexture, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound)
            : this(font, spriteSheetTexture, spriteBatch, width, position, clickSound, null)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="font">The font to use</param>
        /// <param name="spriteSheet">The spritesheet to use</param>
        /// <param name="spriteBatch">The spritebatch to use</param>
        /// <param name="width">The width of the element</param>
        /// <param name="position">The position of the element</param>
        /// <param name="clickSound">The sound if element got clicked</param>
        /// <param name="hoverSound">The sound if mouse hovers over the element</param>
        public AbstractGuiFactory(SpriteFont font, SpritesheetTexture spriteSheetTexture, SpriteBatch spriteBatch, int width, Vector2 position, SoundEffect clickSound, SoundEffect hoverSound)
        {
            this.font = font;
            this.spritesheetTexture = spriteSheetTexture;
            this.spriteBatch = spriteBatch;
            this.width = width;
            this.position = position;
            this.clickSound = clickSound;
            this.hoverSound = hoverSound;
        }

        /// <inheritdoc/>
        public abstract T GetNewObject();
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.DataStructure.Spritesheet;
using Tank.Gui;
using Tank.Wrapper;

namespace Tank.GameStates.States
{
    abstract class AbstractMenuScreen : BaseAbstractState
    {
        protected readonly IDataLoader<SpriteSheet> dataLoader;
        protected SpriteSheet guiSprite;
        protected SpriteFont baseFont;
        protected DataManager<SpriteSheet> spriteSetManager;
        protected SoundEffect buttonClick;

        protected IGuiElement elementToDraw;

        public AbstractMenuScreen()
            : this(new JsonTextureLoader())
        {

        }

        public AbstractMenuScreen(IDataLoader<SpriteSheet> dataLoader)
        {
            this.dataLoader = dataLoader;
        }

        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            spriteSetManager = new DataManager<SpriteSheet>(contentWrapper, dataLoader);
        }

        public override void LoadContent()
        {
            guiSprite = spriteSetManager.GetData("GuiSpriteSheet");
            baseFont = contentWrapper.Content.Load<SpriteFont>("gameFont");
            buttonClick = contentWrapper.Content.Load<SoundEffect>("Sound/Effects/ButtonClick");
        }

        public override void Update(GameTime gameTime)
        {
            elementToDraw.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            TankGame.PublicGraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            elementToDraw.Draw(gameTime);
            spriteBatch.End();
        }
    }
}

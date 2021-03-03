using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.DataStructure.Spritesheet;
using Tank.Gui;
using Tank.Wrapper;

namespace Tank.GameStates.States
{
    class MainMenuState : BaseAbstractState
    {
        private readonly IDataLoader<SpriteSheet> dataLoader;
        private SpriteSheet guiSprite;
        private SpriteFont baseFont;
        private DataManager<SpriteSheet> spriteSetManager;
        private Button exitButton;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public MainMenuState()
            : this(new JsonTextureLoader())
        {
        }

        public MainMenuState(IDataLoader<SpriteSheet> dataLoader)
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
        }

        public override void SetActive()
        {
            exitButton = new Button(new Vector2(100, 100), 100, guiSprite, spriteBatch, baseFont);
            exitButton.Text = "Exit game";
        }

        public override void Update(GameTime gameTime)
        {
            exitButton.Update(gameTime);
            if (exitButton.Clicked)
            {
                gameStateManager.Pop();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            TankGame.PublicGraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            exitButton.Draw(gameTime);
            spriteBatch.End();
        }
    }
}

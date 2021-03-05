using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tank.Commands;
using Tank.Commands.GameManager;
using Tank.Factories;
using Tank.Factories.Gui;
using Tank.Gui;
using Tank.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// The esacpe screen menu
    /// </summary>
    class EscMenuScreen : AbstractMenuScreen
    {
        /// <summary>
        /// Command to use to get back to the main menu
        /// </summary>
        private ICommand mainMenuCommand;

        /// <summary>
        /// Command to use to close this screen
        /// </summary>
        private ICommand closeStateCommand;

        /// <summary>
        /// If the state is currently new
        /// </summary>
        private bool newState;

        /// <summary>
        /// Initialize this class
        /// </summary>
        /// <param name="contentWrapper"></param>
        /// <param name="spriteBatch"></param>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            closeStateCommand = new CloseStateCommand(gameStateManager);
            mainMenuCommand = new RevertToMainMenuCommand(gameStateManager);

        }

        /// <inheritdoc/>
        public override void SetActive()
        {
            base.SetActive();
            newState = true;
            IFactory<Button> buttonFactory = new ButtonFactory(baseFont, guiSprite, spriteBatch, 100, Vector2.Zero, buttonClick, buttonHover);
            Button mainMenu = buttonFactory.GetNewObject();
            mainMenu.SetText("Main menu");
            mainMenu.SetCommand(mainMenuCommand);

            Button back = buttonFactory.GetNewObject();
            back.SetText("Back");
            back.SetClickEffect(buttonClick);
            back.SetCommand(closeStateCommand);

            VerticalStackPanel verticalStackPanel = new VerticalStackPanel(
                new Vector2(0, TankGame.PublicGraphicsDevice.Viewport.Height / 2),
                TankGame.PublicGraphicsDevice.Viewport.Width,
                15,
                true
                );
            verticalStackPanel.SetMouseWrapper(mouseWrapper);
            verticalStackPanel.AddElement(mainMenu);
            verticalStackPanel.AddElement(back);

            elementToDraw = verticalStackPanel;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (newState && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return;
            }
            newState = false;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                closeStateCommand.Execute();
            }
        }
    }
}

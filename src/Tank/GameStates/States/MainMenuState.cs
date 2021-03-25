using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Builders;
using Tank.Commands;
using Tank.Commands.GameManager;
using Tank.DataStructure.Settings;
using Tank.Enums;
using Tank.Factories;
using Tank.Factories.Gui;
using Tank.GameStates.Data;
using Tank.Gui;
using Tank.Map.Generators;
using Tank.Randomizer;
using Tank.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// Main menu state
    /// </summary>
    class MainMenuState : AbstractMenuScreen
    {

        /// <summary>
        /// The close game command
        /// </summary>
        private ICommand closeGameCommand;

        /// <summary>
        /// The start game command
        /// </summary>
        private ICommand startGameCommand;

        /// <summary>
        /// Open the setting menu
        /// </summary>
        private ICommand openSettingCommand;

        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch, ApplicationSettings applicationSettings)
        {
            base.Initialize(contentWrapper, spriteBatch, applicationSettings);
            closeGameCommand = new CloseGameCommand(gameStateManager);
            List<Player> players = new List<Player>();
            List<Rectangle> animationFrames = new List<Rectangle>();
            animationFrames.Add(new Rectangle(0, 0, 32, 32));
            for (int i = 0; i < 4; i++)
            {
                TankObjectBuilder tankObjectBuilder = new TankObjectBuilder(
                    contentWrapper.Load<Texture2D>("Images/Entities/BasicTank"),
                    animationFrames
                 );

                players.Add(
                    new Player(
                        "Player " + (i + 1),
                        i,
                        ControlTypeEnum.Keyboard,
                        i % 2,
                        PlayerTypeEnum.Player,
                        tankObjectBuilder
                        )
                    );
            }
            GameSettings settings = new GameSettings(0.098f, 0f, players, int.MinValue, "MoistContinentalSpritesheet");
#if DEBUG
            settings.SetDebug();
#endif
            IState stateToReplace = new GameLoadingScreen(
                                        new MidpointDisplacementGenerator(
                                            TankGame.PublicGraphicsDevice,
                                            viewportAdapter.VirtualWidth / 4,
                                            0.5f,
                                            new SystemRandomizer()),
                                        settings);
            openSettingCommand = new OpenAdditionalStateCommand(gameStateManager, new SettingState(musicManager), false);
            startGameCommand = new ReplaceStateCommand(gameStateManager, stateToReplace);
        }

        /// <inheritdoc/>
        public override void SetActive()
        {
            base.SetActive();
            IFactory<Button> buttonFactory = new ButtonFactory(baseFont, guiSprite, spriteBatch, 120, Vector2.Zero, buttonClick, buttonHover);
            Button exitButton = buttonFactory.GetNewObject();
            exitButton.SetText("Exit game");
            exitButton.SetCommand(closeGameCommand);

            Button openSettings = buttonFactory.GetNewObject();
            openSettings.SetText("Settings");
            openSettings.SetCommand(openSettingCommand);

            Button startGameButton = buttonFactory.GetNewObject();
            startGameButton.SetText("Start game");
            startGameButton.SetCommand(startGameCommand);

            VerticalStackPanel verticalStackPanel = new VerticalStackPanel(new Vector2(0, 0), viewportAdapter.VirtualViewport.Width / 6, 15, true);
            verticalStackPanel.AddElement(startGameButton);
            verticalStackPanel.AddElement(openSettings);
            verticalStackPanel.AddElement(exitButton);
            verticalStackPanel.SetMouseWrapper(mouseWrapper);

            elementToDraw = verticalStackPanel;

            UpdateUiEffects(settings.EffectVolume);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);startGameCommand.Execute();
        }
    }
}

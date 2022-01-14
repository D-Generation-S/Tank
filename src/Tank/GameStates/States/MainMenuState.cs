using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using Tank.Builders;
using Tank.Commands.GameManager;
using Tank.Enums;
using Tank.GameStates.Data;
using Tank.Map.Generators;
using TankEngine.Commands;
using TankEngine.DataProvider.Loader.String;
using TankEngine.Factories;
using TankEngine.Factories.Gui;
using TankEngine.GameStates.States;
using TankEngine.Gui;
using TankEngine.Randomizer;
using TankEngine.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// Main menu state
    /// </summary>
    class MainMenuState : AbstractMenuScreen
    {
        /// <summary>
        /// The path to the file where the version is saved into
        /// </summary>
        private const string VERSION_PATH = "Tank.Assets.Resources.Version.txt";

        /// <summary>
        /// The path to the file where the version is saved into
        /// </summary>
        private const string ISSUE_URL = "Tank.Assets.Resources.GithubIssueLink.txt";

        /// <summary>
        /// The version of the game
        /// </summary>
        private string version;

        /// <summary>
        /// The url used to open up the issues
        /// </summary>
        private string issueUrl;

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
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            closeGameCommand = new CloseGameCommand(gameStateManager);
            IState stateToReplace = PlaceholderGameSetup(contentWrapper);
            openSettingCommand = new OpenAdditionalStateCommand(gameStateManager, new SettingState(musicManager), false);
            startGameCommand = new ReplaceStateCommand(gameStateManager, stateToReplace);
            ResourceStringDataLoader resourceLoader = new ResourceStringDataLoader();
            version = resourceLoader.LoadData(VERSION_PATH).Trim();
            issueUrl = resourceLoader.LoadData(ISSUE_URL).Trim();
        }

        /// <summary>
        /// This method will setup the game, need to be part of the lobby screen later on.
        /// </summary>
        /// <param name="contentWrapper">The content wrapper to use</param>
        /// <returns></returns>
        private IState PlaceholderGameSetup(ContentWrapper contentWrapper)
        {
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
                                            viewportAdapter.VirtualWidth / 4,
                                            0.5f,
                                            new SystemRandomizer()),
                                        settings);
            return stateToReplace;
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

            VerticalStackPanel verticalStackPanel = new VerticalStackPanel(TankGame.PublicViewportAdapter, new Vector2(0, 0), viewportAdapter.VirtualViewport.Width / 6, 15, true);
            verticalStackPanel.SetMouseWrapper(mouseWrapper);
            verticalStackPanel.AddElement(startGameButton);
            verticalStackPanel.AddElement(openSettings);
            verticalStackPanel.AddElement(exitButton);

            elementToDraw = verticalStackPanel;

            if (version != null && version != string.Empty && baseFont != null)
            {
                AddCustomDraw(gametime =>
                {
                    string versionText = string.Format("Game version: {0}", version);
                    Vector2 textSize = baseFont.MeasureString(versionText);
                    Vector2 versionPosition = new Vector2(viewportAdapter.VirtualWidth, viewportAdapter.VirtualHeight) - textSize;
                    spriteBatch.DrawString(baseFont, versionText, versionPosition, Color.White);
                });
            }

            if (issueUrl == null || issueUrl == string.Empty)
            {
                return;
            }
            Button issueButton = new Button(Vector2.UnitX * viewportAdapter.VirtualWidth - Vector2.UnitX * 170, 150, guiSprite, spriteBatch);
            issueButton.SetFont(baseFont);
            issueButton.SetText("Report bug");
            issueButton.SetMouseWrapper(mouseWrapper);
            issueButton.SetCommand(() =>
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = issueUrl
                };
                Process.Start(startInfo);
            });
            AddCustomDraw(gametime =>
            {
                issueButton.Draw(gametime);
            });
            AddCustomUpdate(gametime => issueButton.Update(gametime));


            //UpdateUiEffects(settings.EffectVolume);
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}

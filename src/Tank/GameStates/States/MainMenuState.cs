using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.Commands;
using Tank.Commands.GameManager;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.DataStructure.Spritesheet;
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
        /// The stack panel to use for data preperation
        /// </summary>
        private VerticalStackPanel verticalStackPanel;

        /// <summary>
        /// The close game command
        /// </summary>
        private ICommand closeGameCommand;

        /// <summary>
        /// The start game command
        /// </summary>
        private ICommand startGameCommand;

        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            closeGameCommand = new CloseGameCommand(gameStateManager);
            GameSettings settings = new GameSettings(0.098f, 0.3f, 4, int.MinValue, "MoistContinentalSpritesheet");
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
            startGameCommand = new ReplaceStateCommand(gameStateManager, stateToReplace);
        }

        /// <inheritdoc/>
        public override void SetActive()
        {
            base.SetActive();
            Button exitButton = new Button(Vector2.Zero, 100, guiSprite, spriteBatch, baseFont);
            exitButton.Text = "Exit game";
            exitButton.SetClickEffect(buttonClick);
            exitButton.SetCommand(closeGameCommand);

            Button startGameButton = new Button(Vector2.Zero, 100, guiSprite, spriteBatch, baseFont);
            startGameButton.Text = "Start game";
            startGameButton.SetClickEffect(buttonClick);
            startGameButton.SetCommand(startGameCommand);

            verticalStackPanel = new VerticalStackPanel(new Vector2(0, 0), viewportAdapter.VirtualViewport.Width / 6, 15, true);
            verticalStackPanel.AddElement(startGameButton);
            verticalStackPanel.AddElement(exitButton);
            verticalStackPanel.SetMouseWrapper(mouseWrapper);

            elementToDraw = verticalStackPanel;
        }


    }
}

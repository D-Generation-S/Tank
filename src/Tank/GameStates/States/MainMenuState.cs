using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
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
    class MainMenuState : AbstractMenuScreen
    {
        private Button exitButton;
        private Button startGameButton;
        private VerticalStackPanel verticalStackPanel;

        public override void SetActive()
        {
            base.SetActive();
            exitButton = new Button(Vector2.Zero, 100, guiSprite, spriteBatch, baseFont);
            exitButton.Text = "Exit game";
            exitButton.SetClickEffect(buttonClick);

            startGameButton = new Button(Vector2.Zero, 100, guiSprite, spriteBatch, baseFont);
            startGameButton.Text = "Start game";
            startGameButton.SetClickEffect(buttonClick);

            verticalStackPanel = new VerticalStackPanel(new Vector2(0, 0), viewportAdapter.VirtualViewport.Width / 6, 15, true);
            verticalStackPanel.AddElement(startGameButton);
            verticalStackPanel.AddElement(exitButton);
            verticalStackPanel.SetMouseWrapper(mouseWrapper);

            elementToDraw = verticalStackPanel;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (startGameButton.Clicked)
            {
                GameSettings settings = new GameSettings(0.098f, 0.3f, 4, int.MinValue, "MoistContinentalSpritesheet");
#if DEBUG
                settings.SetDebug();
#endif
                gameStateManager.Replace(new GameLoadingScreen(
                    new MidpointDisplacementGenerator(
                        TankGame.PublicGraphicsDevice,
                        viewportAdapter.VirtualWidth / 4,
                        0.5f,
                        new SystemRandomizer()),
                    settings));
            }
            if (exitButton.Clicked)
            {
                gameStateManager.Pop();
            }
        }


    }
}

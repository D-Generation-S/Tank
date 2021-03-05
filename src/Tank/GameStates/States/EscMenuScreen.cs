using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.Gui;

namespace Tank.GameStates.States
{
    class EscMenuScreen : AbstractMenuScreen
    {
        private Button mainMenu;
        private Button back;
        private VerticalStackPanel verticalStackPanel;

        public override void SetActive()
        {
            base.SetActive();
            mainMenu = new Button(Vector2.Zero, 100, guiSprite, spriteBatch, baseFont);
            mainMenu.Text = "Main menu";
            mainMenu.SetClickEffect(buttonClick);

            back = new Button(Vector2.Zero, 100, guiSprite, spriteBatch, baseFont);
            back.Text = "Back";
            back.SetClickEffect(buttonClick);

            verticalStackPanel = new VerticalStackPanel(
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
            if (back.Clicked)
            {
                gameStateManager.Pop();
            }
            if (mainMenu.Clicked)
            {
                gameStateManager.ResetState(new MainMenuState());
            }
        }
    }
}

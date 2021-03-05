using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tank.Commands;
using Tank.Commands.GameManager;
using Tank.Factories;
using Tank.Factories.Gui;
using Tank.Gui;
using Tank.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// The settings page
    /// </summary>
    class SettingState : AbstractMenuScreen
    {
        /// <summary>
        /// The command to use to close the setting page
        /// </summary>
        private ICommand closeSettingsCommand;

        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            closeSettingsCommand = new CloseStateCommand(gameStateManager);
        }
        
        /// <inheritdoc/>
        public override void SetActive()
        {
            base.SetActive();
            IFactory<Button> buttonFactory = new ButtonFactory(baseFont, guiSprite, spriteBatch, 100, Vector2.Zero, buttonClick, buttonHover);
            IFactory<Checkbox> checkboxFactory = new CheckboxFactory(baseFont, guiSprite, spriteBatch, 100, Vector2.Zero, buttonClick, buttonHover);
            Button backButton = buttonFactory.GetNewObject();
            backButton.SetText("Back");
            backButton.SetCommand(closeSettingsCommand);

            Checkbox checkbox = checkboxFactory.GetNewObject();
            checkbox.SetText("Fullscreen (Not working placeholder)");
            checkbox.Name = "CB_Fullscreen";
            

            VerticalStackPanel stackPanel = new VerticalStackPanel(
                new Vector2(0, 0),
                TankGame.PublicViewportAdapter.VirtualWidth,
                16,
                true
                );
            stackPanel.SetMouseWrapper(mouseWrapper);
            stackPanel.AddElement(checkbox);
            stackPanel.AddElement(backButton);
            elementToDraw = stackPanel;
        }
    }
}

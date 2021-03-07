using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Tank.Commands;
using Tank.Commands.GameManager;
using Tank.DataStructure.Settings;
using Tank.Factories;
using Tank.Factories.Gui;
using Tank.Gui;
using Tank.Gui.Data;
using Tank.Music;
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

        /// <summary>
        /// The volume selection
        /// </summary>
        private SelectBox volumeSelection;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="manager">The manager to use</param>
        public SettingState(MusicManager manager) : base(manager)
        {
        }

        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch, ApplicationSettings applicationSettings)
        {
            base.Initialize(contentWrapper, spriteBatch, applicationSettings);
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

            volumeSelection = new SelectBox(Vector2.Zero, 100, guiSprite, spriteBatch);
            volumeSelection.SetFont(baseFont);
            volumeSelection.SetClickEffect(buttonClick);
            volumeSelection.SetHoverEffect(buttonHover);
            volumeSelection.SetMouseWrapper(mouseWrapper);
            List<SelectionDataSet> data = new List<SelectionDataSet>();
            for (int i = 0; i < 11; i++)
            {
                int numberOfChars = i + 1;
                string name = new string('0', numberOfChars);
                data.Add(new SelectionDataSet(name, i));
            }
            volumeSelection.SetData(data, data.Count - 1);

            VerticalStackPanel stackPanel = new VerticalStackPanel(
                new Vector2(0, 0),
                TankGame.PublicViewportAdapter.VirtualWidth,
                16,
                true
                );
            stackPanel.SetMouseWrapper(mouseWrapper);
            stackPanel.AddElement(checkbox);
            stackPanel.AddElement(volumeSelection);
            stackPanel.AddElement(backButton);
            elementToDraw = stackPanel;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (volumeSelection.Changed)
            {
                int data = volumeSelection.GetData().GetData<int>();
                settings.MusicVolume = (float)data / 10f;
                MediaPlayer.Volume = settings.MusicVolume;
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Tank.Commands.GameManager;
using Tank.DataStructure.Settings;
using TankEngine.Commands;
using TankEngine.Factories;
using TankEngine.Factories.Gui;
using TankEngine.Gui;
using TankEngine.Gui.Data;
using TankEngine.Music;
using TankEngine.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// The settings page
    /// </summary>
    class SettingState : AbstractMenuScreen
    {
        /// <summary>
        /// The width for each button element
        /// </summary>
        private const int ELEMENT_WIDTH = 170;

        /// <summary>
        /// The command to use to close the setting page
        /// </summary>
        private ICommand closeSettingsCommand;

        /// <summary>
        /// The game resolutions
        /// </summary>
        private SelectBox resolutionSelection;

        /// <summary>
        /// The volume selection
        /// </summary>
        private SelectBox masterVolumeSelection;

        /// <summary>
        /// The music volume selection
        /// </summary>
        private SelectBox musicVolumeSelection;

        /// <summary>
        /// The effect volume selection
        /// </summary>
        private SelectBox effectVolumeSelection;

        /// <summary>
        /// The save button
        /// </summary>
        private Button saveButton;

        /// <summary>
        /// Should be fullscreen
        /// </summary>
        private Checkbox fullScreen;
        private bool restartRequired;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="manager">The manager to use</param>
        public SettingState(MusicManager manager) : base(manager)
        {
        }

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
            IFactory<Button> buttonFactory = new ButtonFactory(baseFont, guiSprite, spriteBatch, ELEMENT_WIDTH, Vector2.Zero, buttonClick, buttonHover);
            IFactory<Checkbox> checkboxFactory = new CheckboxFactory(baseFont, guiSprite, spriteBatch, ELEMENT_WIDTH, Vector2.Zero, buttonClick, buttonHover);
            IFactory<SelectBox> selectionFactory = new SelectionBoxFactory(baseFont, guiSprite, spriteBatch, ELEMENT_WIDTH, Vector2.Zero, buttonClick, buttonHover);
            Button backButton = buttonFactory.GetNewObject();
            backButton.SetText("Back");
            backButton.SetCommand(closeSettingsCommand);

            fullScreen = checkboxFactory.GetNewObject();
            fullScreen.SetText("Fullscreen");
            fullScreen.Name = "CB_Fullscreen";
            fullScreen.Checked = ApplicationSettingsSingelton.Instance.FullScreen;

            resolutionSelection = selectionFactory.GetNewObject();
            masterVolumeSelection = selectionFactory.GetNewObject();
            musicVolumeSelection = selectionFactory.GetNewObject();
            effectVolumeSelection = selectionFactory.GetNewObject();

            List<SelectionDataSet> data = GetVolumeData();
            List<SelectionDataSet> resolutions = GetResolutions();

            resolutionSelection.SetData(resolutions, GetSelectedResolution(resolutions, ApplicationSettingsSingelton.Instance.Resolution));
            resolutionSelection.SetText("Resolution");

            masterVolumeSelection.SetData(data, GetVolumeDataIndex(data, ApplicationSettingsSingelton.Instance.MasterVolumePercent));
            masterVolumeSelection.SetText("Master volume");

            musicVolumeSelection.SetData(data, GetVolumeDataIndex(data, ApplicationSettingsSingelton.Instance.MusicVolumePercent));
            musicVolumeSelection.SetText("Music volume");

            effectVolumeSelection.SetData(data, GetVolumeDataIndex(data, ApplicationSettingsSingelton.Instance.EffectVolumePercent));
            effectVolumeSelection.SetText("Effect volume");


            saveButton = buttonFactory.GetNewObject();
            saveButton.SetCommand(() =>
            {
                ApplicationSettingsSingelton.Instance.FullScreen = fullScreen.Checked;
                ApplicationSettingsSingelton.Instance.Resolution = resolutionSelection.GetData().GetData<Point>();
                ApplicationSettingsSingelton.Instance.MasterVolumePercent = masterVolumeSelection.GetData().GetData<int>();
                ApplicationSettingsSingelton.Instance.MusicVolumePercent = musicVolumeSelection.GetData().GetData<int>();
                ApplicationSettingsSingelton.Instance.EffectVolumePercent = effectVolumeSelection.GetData().GetData<int>();

                ApplicationSettingsSingelton.Instance.Save();
                if (restartRequired)
                {
                    string applicationName = FindExecuteable();
                    if (applicationName == string.Empty)
                    {
                        return;
                    }

                    gameStateManager.Clear();
                    Process.Start(applicationName);
                }
            });
            saveButton.SetText("Save");

            VerticalStackPanel stackPanel = new VerticalStackPanel(
                TankGame.PublicViewportAdapter,
                new Vector2(viewportAdapter.Center.X, 0),
                TankGame.PublicViewportAdapter.VirtualWidth,
                16,
                true,
                true
                );
            stackPanel.SetMouseWrapper(mouseWrapper);
            stackPanel.AddElement(fullScreen);
            stackPanel.AddElement(resolutionSelection);
            stackPanel.AddElement(masterVolumeSelection);
            stackPanel.AddElement(musicVolumeSelection);
            stackPanel.AddElement(effectVolumeSelection);
            stackPanel.AddElement(saveButton);
            stackPanel.AddElement(backButton);
            elementToDraw = stackPanel;

            UpdateUiEffects(ApplicationSettingsSingelton.Instance.EffectVolume);
        }

        /// <inheritdoc/>
        public override void Restore()
        {
            base.Restore();
            List<SelectionDataSet> data = GetVolumeData();
            resolutionSelection.SetCurrentDataSet(GetSelectedResolution(GetResolutions(), ApplicationSettingsSingelton.Instance.Resolution));
            masterVolumeSelection.SetCurrentDataSet(GetVolumeDataIndex(data, ApplicationSettingsSingelton.Instance.MasterVolumePercent));
            musicVolumeSelection.SetCurrentDataSet(GetVolumeDataIndex(data, ApplicationSettingsSingelton.Instance.MusicVolumePercent));
            effectVolumeSelection.SetCurrentDataSet(GetVolumeDataIndex(data, ApplicationSettingsSingelton.Instance.EffectVolumePercent));

        }

        /// <inheritdoc/>
        private List<SelectionDataSet> GetResolutions()
        {
            List<SelectionDataSet> returnData = new List<SelectionDataSet>();
            returnData.Add(new SelectionDataSet("1280x720", new Point(1280, 720)));
            returnData.Add(new SelectionDataSet("1920x1080", new Point(1920, 1080)));
            return returnData;
        }

        /// <inheritdoc/>
        private int GetSelectedResolution(List<SelectionDataSet> dataSet, Point savedResolution)
        {
            int returnIndex = dataSet.FindIndex(item =>
            {
                Point data = item.GetData<Point>();
                return data.X == savedResolution.X && data.Y == savedResolution.Y;
            });
            return returnIndex == -1 ? 0 : returnIndex;
        }

        /// <summary>
        /// Get the volume data
        /// </summary>
        /// <returns>The data to set the volume</returns>
        private List<SelectionDataSet> GetVolumeData()
        {
            List<SelectionDataSet> returnData = new List<SelectionDataSet>();
            for (int i = 0; i < 21; i++)
            {
                int halfData = i % 2;
                int fullData = i - halfData;
                string name = new string('O', fullData / 2);
                name += new string('C', halfData);
                name = name == string.Empty ? "Off" : name;
                returnData.Add(new SelectionDataSet(name, i * 10));
            }
            return returnData;
        }

        /// <summary>
        /// Get the volume data index
        /// </summary>
        /// <param name="dataSet">The dataset to search in</param>
        /// <param name="configuredVolume">The current volume</param>
        /// <returns>The index in the data set</returns>
        private int GetVolumeDataIndex(List<SelectionDataSet> dataSet, int configuredVolume)
        {
            int returnIndex = dataSet.FindIndex(item => item.GetData<int>() == configuredVolume);
            return returnIndex == -1 ? dataSet.Count - 1 : returnIndex;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Point newResolution = resolutionSelection.GetData().GetData<Point>();
            restartRequired = ApplicationSettingsSingelton.Instance.FullScreen != fullScreen.Checked;
            restartRequired = restartRequired || ApplicationSettingsSingelton.Instance.Resolution != newResolution;

            saveButton.SetText("Save");
            if (restartRequired)
            {
                saveButton.SetText("Save & Restart");
            }

            if (masterVolumeSelection.Changed)
            {
                float masterDataPercent = masterVolumeSelection.GetData().GetData<float>();
                float musicDataPercent = musicVolumeSelection.GetData().GetData<float>();
                float effectDataPercent = effectVolumeSelection.GetData().GetData<float>();
                float masterData = (float)masterDataPercent / 100f;
                float musicData = (float)musicDataPercent / 100f;
                float effectData = (float)effectDataPercent / 100f;

                MediaPlayer.Volume = musicData * masterData;
                UpdateUiEffects(effectData * masterData);
            }
            if (musicVolumeSelection.Changed)
            {
                float masterDataPercent = masterVolumeSelection.GetData().GetData<float>();
                float masterData = (float)masterDataPercent / 100f;
                float dataPercent = musicVolumeSelection.GetData().GetData<float>();
                float musicData = (float)dataPercent / 100f;
                MediaPlayer.Volume = musicData * masterData;
            }
            if (effectVolumeSelection.Changed)
            {
                float masterDataPercent = masterVolumeSelection.GetData().GetData<float>();
                float masterData = (float)masterDataPercent / 100f;
                float dataPercent = effectVolumeSelection.GetData().GetData<float>();
                float effectData = (float)dataPercent / 100f;
                UpdateUiEffects(effectData * masterData);
            }
        }

        /// <summary>
        /// Find the executeable file for restarting
        /// </summary>
        /// <returns>The path to the executable</returns>
        private string FindExecuteable()
        {
            string applicationAssembly = Assembly.GetEntryAssembly().Location;
            FileInfo info = new FileInfo(applicationAssembly);
            foreach (string file in Directory.GetFiles(info.DirectoryName))
            {
                if (file.EndsWith(".exe"))
                {
                    return file;
                }
            }

            return string.Empty;
        }
    }
}

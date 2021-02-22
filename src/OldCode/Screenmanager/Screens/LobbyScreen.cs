using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Tank.Code.GUIClasses;
using Tank.Code.JSonClasses;
using Tank.Enums;
using System;
using Tank.Code.General;
using Newtonsoft.Json;
using System.Linq;

namespace Tank.Code.Screenmanager
{
    internal class LobbyScreen : BasicScreen
    {
        private List<jsonTank> _availableTanks;

        private int _playerCount = 2;
        private int _tankIndex;
        private int _maxIndex;

        public LobbyScreen(int ScreenWidth, int ScreenHeigh, GraphicsDevice GD) : base(ScreenType.Menu, ScreenWidth, ScreenHeigh, GD)
        {
            Name = "lobby";
            TrackType = TrackType.Menu;
            _availableTanks = new List<jsonTank>();
            _maxIndex = 0;

            _guiEvents.Add("StartGame_Click", StartGame_Click);
            _guiEvents.Add("Back_Click", Back_Click);
            _guiEvents.Add("RightArrow_Click", RightArrow_Click);
            _guiEvents.Add("LeftArrow_Click", LeftArrow_Click);
            _guiEvents.Add("AddPlayerClick", AddPlayerClick);
            _guiEvents.Add("RemovePlayerClick", RemovePlayerClick);
        }

        public override void ActivateScreen(bool FirstScreen = false)
        {
            _availableTanks = new List<jsonTank>();
            LoadAvailableTanks();
            
            base.ActivateScreen(FirstScreen);

            if (_availableTanks.Count > 0)
            {
                _tankIndex = 0;
                SetupTankIcon(_availableTanks[_tankIndex]);
                SetTankDescription(_availableTanks[_tankIndex]);
            }
        }
        
        private void LoadAvailableTanks()
        {
            string tankJson = CodeHelper.LoadJson("Entities\\tanks");

            jsonTankList tanks = JsonConvert.DeserializeObject<jsonTankList>(tankJson);

            tanks.List.ForEach(path =>
            {
                string tempTank = CodeHelper.LoadJson("Entities\\" + path);
                jsonTank temp = JsonConvert.DeserializeObject<jsonTank>(tempTank);
                if (temp != null)
                    _availableTanks.Add(temp);
            });
            _maxIndex = _availableTanks.Count - 1;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            ScreenManager.Instance.SetLastScreen();
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            List<Player> _PlayerList = new List<Player>();
            for (int i = 0; i < _playerCount; i++)
            {
                _PlayerList.Add(new Player(0, 0, i, _availableTanks[_tankIndex].CreateVehicle(), $"Player {i}"));
            }
            BasicScreen Game = new GameScreen(Settings.MaxWindowSize.Width, Settings.MaxWindowSize.Height, _graphicDevice, _PlayerList, Helper.LoadCustomMap("MyMap", _graphicDevice));
            ScreenManager.Instance.SetCurrentScreen(Game, true);
        }

        private void RightArrow_Click(object sender, EventArgs e)
        {
            if (_availableTanks.Count > 0)
            {
                _tankIndex++;
                if (_tankIndex > _maxIndex)
                    _tankIndex = 0;

                SetupTankIcon(_availableTanks[_tankIndex]);
                SetTankDescription(_availableTanks[_tankIndex]);
            }
        }

        private void LeftArrow_Click(object sender, EventArgs e)
        {
            if (_availableTanks.Count > 0)
            {
                _tankIndex--;
                if (_tankIndex < 0)
                    _tankIndex = _maxIndex;

                SetupTankIcon(_availableTanks[_tankIndex]);
                SetTankDescription(_availableTanks[_tankIndex]);

            }
        }

        private void SetTankDescription(jsonTank tank)
        {
            GUITextarea txt = _textareas.Where(text => text.Name == "Textarea_TankDescription").FirstOrDefault();
            if (txt != null)
            {
                string description = string.Empty;
                description += $"Name: {tank.Name + Environment.NewLine}";
                description += $"Min Power: {tank.minPower + Environment.NewLine}";
                description += $"Max Power: {tank.maxPower + Environment.NewLine}";
                description += $"Armor: {tank.Armor + Environment.NewLine}";
                description += Environment.NewLine;
                description += $"Description: {tank.Description + Environment.NewLine}";

                txt.Text = description;
            }
        }

        private void AddPlayerClick(object sender, EventArgs e)
        {
            _playerCount++;
            _playerCount = MathHelper.Clamp(_playerCount, 2, 4);
            GUITextarea temp = _textareas.Where(txt => txt.Name == "Textarea_Playercount").FirstOrDefault();
            if (temp != null)
                temp.Text = _playerCount.ToString();
        }
        private void RemovePlayerClick(object sender, EventArgs e)
        {
            _playerCount--;
            _playerCount = MathHelper.Clamp(_playerCount, 2, 4);
            GUITextarea temp = _textareas.Where(txt => txt.Name == "Textarea_Playercount").FirstOrDefault();
            if (temp != null)
                temp.Text = _playerCount.ToString();
        }

        private void SetupTankIcon(jsonTank CurrentTank)
        {
            GUIPanel panel = _panels.Where(p => p.Name == "Panel_TankPreview").FirstOrDefault();
            if (panel != null)
            {
                Texture2D baseText = TankGame.PublicContentManager.Load<Texture2D>(CurrentTank.BaseTexture);
                Texture2D canon = TankGame.PublicContentManager.Load<Texture2D>(CurrentTank.CanonTexture);
                Texture2D textNew = baseText.Combine(canon, CurrentTank.CanonPosition);
                textNew.Name = baseText.Name;
                panel.PanelContent = textNew;
            }
        }

        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameActive)
        {
            ActiveHandler.Instance.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState);
            base.Update(CurrentMouseState, CurrentKeyboardState, CurrentGameTime, CurrentGamePadState, GameActive);
        }
    }
}
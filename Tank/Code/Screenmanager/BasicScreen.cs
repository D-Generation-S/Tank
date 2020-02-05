using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tank.Interfaces;
using Tank.Code.Sound;
using Tank.Code.JSonClasses;
using System.Reflection;
using Tank.Code.General;
using Tank.Enums;
using Tank.Code.GUIClasses;
using Newtonsoft.Json;

namespace Tank.Code.Screenmanager
{
    public class BasicScreen : IScreenObj
    {
        protected Dictionary<string, Action<object, EventArgs>> _guiEvents;
        protected List<GUIButton> _buttons;
        protected List<GUIPanel> _panels;
        protected List<GUITextBox> _textboxes;
        protected List<GUITextarea> _textareas;
        protected List<GUISlider> _sliders;
        protected List<GUIToggle> _toggles;

        private ScreenType _screenType;
        private Texture2D _overlayTexture;
        protected bool _fillBackGround;
        private Color BackGroundFillColor;
        public Color FillColor
        {
            get
            {
                return BackGroundFillColor;
            }
            set
            {
                BackGroundFillColor = value;
            }
        }
        private int _screenWidth;
        protected int ScreenWidth
        {
            get
            {
                return _screenWidth;
            }
        }
        private int _screenHeight;
        protected int ScreenHeight
        {
            get
            {
                return _screenHeight;
            }
        }
        private Texture2D _BackgroundImage;
        public Texture2D BackgroundImage
        {
            get
            {
                return _BackgroundImage;
            }
            set
            {
                _BackgroundImage = value;
            }
        }

        public string Name
        {
            get;
            set;
        }

        public bool IsBackgroundScreen;

        private bool Active = false;
        internal bool _firstCall;

        private bool _drawCursor;
        public bool ForceDrawCursor;

        private List<IRenderObj> StoredRenderObjects;
        private List<IActiveObj> StoredActiveObjects;
        private List<IPhysicsObj> StoredPhysicsObjects;

        private int mouseX;
        private int mouseY;

        protected GraphicsDevice _graphicDevice;

        public bool Delete;

        protected bool _playMusic;
        public bool PlayMusic
        {
            set
            {
                _playMusic = value;
            }
            get
            {
                return _playMusic;
            }
        }
        public TrackType TrackType
        {
            get;
            set;
        }

        public BasicScreen()
        {
            _graphicDevice = TankGame.PublicGraphicsDevice;
            _screenWidth = Settings.MaxWindowSize.Width;
            _screenHeight = Settings.MaxWindowSize.Height;
            _fillBackGround = true;
            ForceDrawCursor = false;
            StoredActiveObjects = new List<IActiveObj>();
            StoredPhysicsObjects = new List<IPhysicsObj>();
            StoredRenderObjects = new List<IRenderObj>();
            _buttons = new List<GUIButton>();
            _panels = new List<GUIPanel>();
            _textboxes = new List<GUITextBox>();
            _textareas = new List<GUITextarea>();
            _sliders = new List<GUISlider>();
            _toggles = new List<GUIToggle>();
            _guiEvents = new Dictionary<string, Action<object, EventArgs>>();
            _firstCall = true;
            IsBackgroundScreen = false;
            Delete = false;
        }

        public BasicScreen(ScreenType screentype, int ScreenWidth, int ScreenHeigh, GraphicsDevice GD)
        {
            _screenType = screentype;
            _fillBackGround = true;
            ForceDrawCursor = false;
            switch (_screenType)
            {
                case ScreenType.GameOverlay:
                    _fillBackGround = false;
                    _drawCursor = true;
                    GenerateImage(GD);
                    TrackType = TrackType.Menu;
                    break;
                case ScreenType.Menu:
                    _drawCursor = true;
                    TrackType = TrackType.Menu;
                    break;
                case ScreenType.Game:
                    _drawCursor = false;
                    TrackType = TrackType.Battle;
                    break;
                default:
                    break;
            }
            _screenWidth = ScreenWidth;
            _screenHeight = ScreenHeigh;

            StoredActiveObjects = new List<IActiveObj>();
            StoredPhysicsObjects = new List<IPhysicsObj>();
            StoredRenderObjects = new List<IRenderObj>();
            _buttons = new List<GUIButton>();
            _panels = new List<GUIPanel>();
            _textboxes = new List<GUITextBox>();
            _textareas = new List<GUITextarea>();
            _sliders = new List<GUISlider>();
            _toggles = new List<GUIToggle>();
            _guiEvents = new Dictionary<string, Action<object, EventArgs>>();
            _graphicDevice = GD;
            _firstCall = true;
            IsBackgroundScreen = false;
            Delete = false;
        }

        private void GenerateImage(GraphicsDevice GD)
        {
            Color[] colorData = new Color[1];
            colorData[0] = Color.Black;
            colorData[0].A = 225;

            Texture2D OverlayTexture = new Texture2D(GD, 1, 1);
            OverlayTexture.SetData<Color>(colorData);
            _overlayTexture = OverlayTexture;
        }

        virtual public void Draw(SpriteBatch SB, GraphicsDevice GD)
        {
            if (_fillBackGround)
                GD.Clear(BackGroundFillColor);
            if (BackgroundImage != null)
            {
                SB.Draw(BackgroundImage, Settings.MaxWindowSize, Color.White);
            }
            Renderer.Instance.Draw(SB);

            if (_screenType == ScreenType.GameOverlay)
                SB.Draw(_overlayTexture, new Rectangle(0, 0, _screenWidth, _screenHeight), Color.White);

            Renderer.Instance.DrawOverlayer(SB);

            if (_drawCursor || ForceDrawCursor)
            {
                SB.Draw(Settings.GameCursor, new Vector2(mouseX, mouseY));
            }
        }

        virtual public void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime CurrentGameTime, GamePadState CurrentGamePadState, bool GameActive)
        {
            mouseX = (int)MouseHandler.Position.X;
            mouseY = (int)MouseHandler.Position.Y;
            if (CurrentKeyboardState.GetPressedKeys().Contains(Keys.F5))
                LoadScreenDefinition();
        }

        virtual public void ActivateScreen(bool FirstScreen = false)
        {
            if (Delete)
                return;
            if (!Active && !FirstScreen && !_firstCall)
            {
                lock (ActiveHandler.Instance.objects)
                    ActiveHandler.Instance.objects = StoredActiveObjects.ToArray().ToList<IActiveObj>();
                lock (Physics.Instance.objects)
                    Physics.Instance.objects = StoredPhysicsObjects.ToArray().ToList<IPhysicsObj>();
                lock (Renderer.Instance.objects)
                    Renderer.Instance.objects = StoredRenderObjects.ToArray().ToList<IRenderObj>();

                StoredActiveObjects.Clear();
                StoredPhysicsObjects.Clear();
                StoredRenderObjects.Clear();

                if (IsBackgroundScreen)
                    IsBackgroundScreen = false;
            }
            TrackManager.Instance.Category = TrackType;
            TrackManager.Play();
            Active = true;
            _firstCall = false;
            LoadScreenDefinition();
        }

        virtual public void DisableScreen()
        {
            if (Active)
            {
                lock (ActiveHandler.Instance.objects)
                {
                    StoredActiveObjects = (ActiveHandler.Instance.objects).ToArray().ToList<IActiveObj>();
                    ActiveHandler.Instance.objects.Clear();
                }
                lock (Physics.Instance.objects)
                {
                    StoredPhysicsObjects = Physics.Instance.objects.ToArray().ToList<IPhysicsObj>();
                    Physics.Instance.objects.Clear();
                }
                if (!IsBackgroundScreen)
                {
                    lock (Renderer.Instance.objects)
                    {
                        StoredRenderObjects = Renderer.Instance.objects.ToArray().ToList<IRenderObj>();
                        Renderer.Instance.objects.Clear();
                    }
                }
                else
                {
                    StoredRenderObjects = Renderer.Instance.objects.ToArray().ToList<IRenderObj>();
                }
            }

            Active = false;
        }

        public static T GetSubScreen<T>(ScreenDefinition definition) where T : BasicScreen
        {
            Type screenType = Type.GetType(definition.Class);
            object returnType = Activator.CreateInstance(screenType);

            setScreenProperty("Name", definition.Name, ref returnType, screenType);
            setScreenProperty("TrackType", definition.TrackType, ref returnType, screenType);

            return (T)returnType;
        }
        private static void setScreenProperty(string propertyName, string value, ref object o, Type t)
        {
            PropertyInfo pi = t.GetProperty(propertyName, BindingFlags.FlattenHierarchy |
                          BindingFlags.Instance |
                          BindingFlags.Public);
            if (pi != null)
            {
                if (pi.PropertyType == typeof(string))
                {
                    pi.SetValue(o, value);
                }
                else if (pi.PropertyType.IsEnum)
                {
                    try
                    {
                        pi.SetValue(o, Enum.Parse(pi.PropertyType, value));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }



        protected int getActualSize(string size, bool horizontal)
        {
            int iReturn = 0;
            int max = horizontal ? Settings.MaxWindowSize.Width : Settings.MaxWindowSize.Height;

            if (!string.IsNullOrEmpty(size))
            {
                if (size.Contains('%'))
                {
                    float percentage = 0.0f;
                    if (float.TryParse(size.Replace("%", string.Empty), out percentage))
                    {
                        iReturn = (int)Math.Floor(max * (percentage / 100));
                    }
                }
                else
                {
                    int.TryParse(size, out iReturn);
                }
            }

            return iReturn;
        }

        protected int AlignVertical(string verticalAlignment, int buttonHeight, int topMargin, int bottomMargin)
        {
            int posY = 0;

            switch (verticalAlignment)
            {
                case "Top":
                    posY = 0;
                    posY += topMargin;
                    break;
                case "Middle":
                    posY = Settings.MaxWindowSize.Height / 2 - buttonHeight / 2;
                    posY -= bottomMargin;
                    posY += topMargin;
                    break;
                case "Bottom":
                    posY = Settings.MaxWindowSize.Height - buttonHeight;
                    posY -= bottomMargin;
                    break;
                default:
                    posY = 0;
                    posY -= bottomMargin;
                    posY += topMargin;
                    break;
            }

            return posY;
        }

        protected int AlignHorizontal(string horizontalAlignment, int buttonWidth, int rightMargin, int leftMargin)
        {
            int posX = 0;

            switch (horizontalAlignment)
            {
                case "Left":
                    posX = 0;
                    posX += leftMargin;
                    break;
                case "Center":
                    posX = Settings.MaxWindowSize.Width / 2 - buttonWidth / 2;
                    posX -= rightMargin;
                    posX += leftMargin;
                    break;
                case "Right":
                    posX = Settings.MaxWindowSize.Width - buttonWidth;
                    posX -= rightMargin;
                    break;
                default:
                    posX = 0;
                    posX -= rightMargin;
                    posX += leftMargin;
                    break;
            }

            return posX;
        }

        private TextPosition GetTextPosition(string textAlignment)
        {
            TextPosition CurrentPos = TextPosition.Left;
            switch (textAlignment.ToUpper())
            {
                case "LEFT":
                    CurrentPos = TextPosition.Left;
                    break;
                case "CENTER":
                    CurrentPos = TextPosition.Center;
                    break;
                case "RIGHT":
                    CurrentPos = TextPosition.Right;
                    break;
                default:
                    break;
            }
            return CurrentPos;
        }

        protected void LoadButtonList(jsonScreenDefinition screenDef)
        {
            screenDef.Buttons.ForEach(button =>
            {
                int actualWidth = getActualSize(button.Width, true);
                int actualHeight = getActualSize(button.Height, false);
                int posX = AlignHorizontal(button.HorizontalAlignment, actualWidth, button.Margin[1], button.Margin[3]);
                int posY = AlignVertical(button.VerticalAlignment, actualHeight, button.Margin[0], button.Margin[2]);

                Texture2D texture = null;
                if (!string.IsNullOrEmpty(button.TextureName))
                    texture = TankGame.PublicContentManager.Load<Texture2D>(button.TextureName);
                Texture2D hoverTexture = null;
                if (!string.IsNullOrEmpty(button.HoverTextureName))
                    hoverTexture = TankGame.PublicContentManager.Load<Texture2D>(button.HoverTextureName);
                GUIButton temp = new GUIButton(posX, posY, actualWidth, actualHeight, _graphicDevice, texture, button.Text, Settings.GlobalFont, hoverTexture, Settings.ClickSound)
                {
                    InternalData = button.InternalData
                };
                if (temp != null)
                {
                    temp.Name = button.Name;
                    if (!string.IsNullOrEmpty(button.ClickEvent) && _guiEvents.ContainsKey(button.ClickEvent))
                    {
                        Action<object, EventArgs> clickEvent = _guiEvents[button.ClickEvent];
                        if (clickEvent != null)
                            temp.Click += (sender, args) => clickEvent(sender, args);
                    }
                    _buttons.Add(temp);
                }
            });
        }
        protected void LoadPanelList(jsonScreenDefinition screenDef)
        {
            screenDef.Panels.ForEach(panel =>
            {
                int actualWidth = getActualSize(panel.Width, true);
                int actualHeight = getActualSize(panel.Height, false);
                int posX = AlignHorizontal(panel.HorizontalAlignment, actualWidth, panel.Margin[1], panel.Margin[3]);
                int posY = AlignVertical(panel.VerticalAlignment, actualHeight, panel.Margin[0], panel.Margin[2]);

                Texture2D texture = null;
                if (!string.IsNullOrEmpty(panel.TextureName))
                    texture = TankGame.PublicContentManager.Load<Texture2D>(panel.TextureName);
                GUIPanel temp = new GUIPanel(posX, posY, actualWidth, actualHeight, texture);
                temp.Name = panel.Name;
                _panels.Add(temp);
            });
        }
        protected void LoadTextboxList(jsonScreenDefinition screenDef)
        {
            screenDef.Textboxes.ForEach(textbox =>
            {
                int actualWidth = getActualSize(textbox.Width, true);
                int actualHeight = getActualSize(textbox.Height, false);
                int posX = AlignHorizontal(textbox.HorizontalAlignment, actualWidth, textbox.Margin[1], textbox.Margin[3]);
                int posY = AlignVertical(textbox.VerticalAlignment, actualHeight, textbox.Margin[0], textbox.Margin[2]);

                Texture2D texture = null;
                if (!string.IsNullOrEmpty(textbox.TextureName))
                    texture = TankGame.PublicContentManager.Load<Texture2D>(textbox.TextureName);
                Texture2D activeTexture = null;
                if (!string.IsNullOrEmpty(textbox.ActiveTextureName))
                    activeTexture = TankGame.PublicContentManager.Load<Texture2D>(textbox.ActiveTextureName);

                GUITextBox temp = new GUITextBox(posX, posY, actualWidth, actualHeight, texture, activeTexture);
                temp.Name = textbox.Name;
                temp.DrawTextPosition = GetTextPosition(textbox.TextAlignment);
                _textboxes.Add(temp);
            });
        }
        protected void LoadTextAreaList(jsonScreenDefinition screenDef)
        {
            screenDef.Textareas.ForEach(textarea =>
            {
                int actualWidth = getActualSize(textarea.Width, true);
                int actualHeight = getActualSize(textarea.Height, false);
                int posX = AlignHorizontal(textarea.HorizontalAlignment, actualWidth, textarea.Margin[1], textarea.Margin[3]);
                int posY = AlignVertical(textarea.VerticalAlignment, actualHeight, textarea.Margin[0], textarea.Margin[2]);

                Texture2D texture = null;
                if (!string.IsNullOrEmpty(textarea.TextureName))
                    texture = TankGame.PublicContentManager.Load<Texture2D>(textarea.TextureName);

                GUITextarea temp = new GUITextarea(posX, posY, actualWidth, actualHeight, textarea.Padding)
                {
                    CurrentTexture = texture,
                    Name = textarea.Name,
                    Text = textarea.Text,
                    TextColor = CodeHelper.ConvertFromHex(textarea.FontColor)
                };
                _textareas.Add(temp);
            });
        }
        protected void LoadSliderList(jsonScreenDefinition screenDef)
        {
            screenDef.Sliders.ForEach(slider =>
            {
                int actualWidth = getActualSize(slider.Width, true);
                int actualHeight = getActualSize(slider.Height, false);
                int posX = AlignHorizontal(slider.HorizontalAlignment, actualWidth, slider.Margin[1], slider.Margin[3]);
                int posY = AlignVertical(slider.VerticalAlignment, actualHeight, slider.Margin[0], slider.Margin[2]);

                Texture2D backgroundTexture = null;
                if (!string.IsNullOrEmpty(slider.BackgroundTextureName))
                    backgroundTexture = TankGame.PublicContentManager.Load<Texture2D>(slider.BackgroundTextureName);
                Texture2D sliderTexture = null;
                if (!string.IsNullOrEmpty(slider.SliderTextureName))
                    sliderTexture = TankGame.PublicContentManager.Load<Texture2D>(slider.SliderTextureName);

                GUISlider temp = new GUISlider(posX, posY, actualWidth, actualHeight)
                {
                    CurrentTexture = backgroundTexture,
                    SliderTexture = sliderTexture,
                    SliderHeight = slider.SliderHeight,
                    SliderWidth = slider.SliderWidth,
                    SliderOffsetX = slider.SliderOffset[0],
                    SliderOffsetY = slider.SliderOffset[1],
                    Name = slider.Name
                };

                if (!string.IsNullOrEmpty(slider.ChangeEvent) && _guiEvents.ContainsKey(slider.ChangeEvent))
                {
                    Action<object, EventArgs> clickEvent = _guiEvents[slider.ChangeEvent];
                    if (clickEvent != null)
                        temp.OnChange += (sender, args) => clickEvent(sender, args);
                }
                temp.Initialize();
                _sliders.Add(temp);
            });
        }
        protected void LoadToggleList(jsonScreenDefinition screenDef)
        {
            screenDef.Toggles.ForEach(toggle =>
            {
                int actualWidth = getActualSize(toggle.Width, true);
                int actualHeight = getActualSize(toggle.Height, false);
                int posX = AlignHorizontal(toggle.HorizontalAlignment, actualWidth, toggle.Margin[1], toggle.Margin[3]);
                int posY = AlignVertical(toggle.VerticalAlignment, actualHeight, toggle.Margin[0], toggle.Margin[2]);

                Texture2D texture = null;
                if (!string.IsNullOrEmpty(toggle.TextureName))
                    texture = TankGame.PublicContentManager.Load<Texture2D>(toggle.TextureName);
                Texture2D checkedTexture = null;
                if (!string.IsNullOrEmpty(toggle.CheckedTextureName))
                    checkedTexture = TankGame.PublicContentManager.Load<Texture2D>(toggle.CheckedTextureName);
                GUIToggle temp = new GUIToggle(posX, posY, actualWidth, actualHeight, checkedTexture, texture, toggle.Text, Settings.GlobalFont, Settings.ClickSound, PossibleTextPosition.Right)
                {
                    Name = toggle.Name
                };
                if (!string.IsNullOrEmpty(toggle.ClickEvent) && _guiEvents.ContainsKey(toggle.ClickEvent))
                {
                    Action<object, EventArgs> clickEvent = _guiEvents[toggle.ClickEvent];
                    if (clickEvent != null)
                        temp.Changing += (sender, args) => clickEvent(sender, args);
                }

                if (temp != null)
                {
                    temp.Name = toggle.Name;

                    _toggles.Add(temp);
                }
            });
        }


        private void LoadScreenDefinition()
        {
            _buttons.ForEach(button =>
            {
                button.Dispose();
            });
            _panels.ForEach(panel =>
            {
                panel.Dispose();
            });
            _textboxes.ForEach(textbox =>
            {
                textbox.Dispose();
            });
            _textareas.ForEach(textarea =>
            {
                textarea.Dispose();
            });
            _sliders.ForEach(slider =>
            {
                slider.Dispose();
            });
            _toggles.ForEach(toggle =>
            {
                toggle.Dispose();
            });
            _buttons.Clear();
            _panels.Clear();
            _textboxes.Clear();
            _textareas.Clear();
            _sliders.Clear();
            _toggles.Clear();

            try
            {
                string screenDefinitionJson = CodeHelper.LoadJson($"GUI\\{Name}");
                jsonScreenDefinition screenDef = JsonConvert.DeserializeObject<jsonScreenDefinition>(screenDefinitionJson);
                LoadButtonList(screenDef);
                LoadPanelList(screenDef);
                LoadTextboxList(screenDef);
                LoadTextAreaList(screenDef);
                LoadSliderList(screenDef);
                LoadToggleList(screenDef);
                if (!string.IsNullOrEmpty(screenDef.BackgroundTextureName))
                    BackgroundImage = TankGame.PublicContentManager.Load<Texture2D>(screenDef.BackgroundTextureName);
                else
                    BackgroundImage = null;
                if (!string.IsNullOrEmpty(screenDef.BackgroundColor))
                    FillColor = CodeHelper.ConvertFromHex(screenDef.BackgroundColor);
                else
                    FillColor = CodeHelper.ConvertFromHex("#222");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BasicScreen: {ex.Message}");
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Tank.Code.GUIClasses
{


    public class GUITextBox : GUIPrimitiv
    {
        private string InternalText;
        public string Text
        {
            get
            {
                return InternalText;
            }
            set
            {
                InternalText = value;
            }
        }
        public int TextLenght
        {
            get
            {
                return InternalText.Length;
            }
        }
        private string ShownText;

        private Texture2D t2dMainTexture;
        private Texture2D t2dActiveTexture;
        private Texture2D t2dBlinkingMarker;

        public int BlinkingCursorWidth;
        public int BlinkingCursorHeight;
        public int BlinkSpeed;
        private int CurrentBlinkTime;
        private bool DrawBlink;

        private Color TextColor;
        public Color FontColor
        {
            get
            {
                return TextColor;
            }
            set
            {
                TextColor = value;
            }
        }
        public int LeftXOffset;
        private int CurrentXOffset;
        private KeyboardState previousState;
        private Keys[] PreviousKeys;

        private double timer;

        private int CurrentDelTick;
        private bool FirstClick;
        private int CurrentKeyTick;
        private bool UpperCase;
        private int CurrentShiftTick;
        private int DelTick;

        private bool _focused;
        public bool Focused
        {
            get
            {
                return _focused;
            }
        }

        private bool MouseClicked;
        private MouseState LastMouseState;

        public event EventHandler TextChanged;
        public event EventHandler Click;

        public TextPosition DrawTextPosition;

        public GUITextBox(int PositionX, int PositionY, int Width, int Height, Texture2D MainTexture, Texture2D ActiveTexture, Texture2D BlinkingCursor = null, bool Overlayer = false)
            : base(PositionX, PositionY, Width, Height, MainTexture, Overlayer)
        {
            t2dMainTexture = MainTexture;
            t2dActiveTexture = ActiveTexture;
            t2dBlinkingMarker = BlinkingCursor;
            CurrentBlinkTime = 0;
            DrawBlink = true;
            BlinkSpeed = 50;
            InternalText = "";
            ShownText = "";
            LeftXOffset = 3;
            CurrentXOffset = 0;
            TextColor = Color.Black;
            DelTick = 5;
            CurrentDelTick = 0;
            CurrentKeyTick = 0;
            CurrentShiftTick = 0;
            FirstClick = true;
            BlinkingCursorHeight = (int)Settings.GlobalFont.MeasureString("Z").Y;
            BlinkingCursorWidth = 2;
            UpperCase = false;
        }

        public override void Update(MouseState CurrentMouseState, KeyboardState CurrentKeyboardState, GameTime currentGameTime, GamePadState gsState)
        {
            if (_hidden)
                return;
            if (_focused)
                GetInput(currentGameTime, CurrentKeyboardState);

            HandleCursor(CurrentKeyboardState);
            HandleFocusLogic(CurrentMouseState);
            GenerateShownText();
            LastMouseState = CurrentMouseState;
        }

        private void HandleCursor(KeyboardState CurrentKeyboardState)
        {
            if (_focused)
            {
                if (CurrentBlinkTime >= BlinkSpeed)
                {
                    DrawBlink = false;
                }
                else if (CurrentBlinkTime <= 0)
                {
                    DrawBlink = true;
                }
                if (DrawBlink)
                {
                    CurrentBlinkTime++;
                }
                else
                {
                    CurrentBlinkTime--;
                }
            }
            else
            {
                CurrentTexture = t2dMainTexture;
                DrawBlink = false;
            }
        }

        private void HandleFocusLogic(MouseState CurrentMouseState)
        {
            if (Element.Contains(MouseHandler.Position))
            {
                if (!MouseClicked && MouseHandler.LeftButton == ButtonState.Pressed)
                {
                    MouseClicked = true;
                }
                if (MouseClicked && MouseHandler.LeftButton == ButtonState.Released)
                {
                    Click?.Invoke(this, new EventArgs());
                    MouseClicked = false;
                    _focused = true;
                    CurrentTexture = t2dActiveTexture;
                }
            }
            else
            {
                if (MouseClicked && MouseHandler.LeftButton == ButtonState.Released)
                    MouseClicked = false;
                if (_focused && MouseHandler.LeftButton == ButtonState.Pressed && MouseHandler.LeftButton == ButtonState.Released)
                {
                    _focused = false;
                }

            }
        }

        private void GetInput(GameTime gameTime, KeyboardState CurrentKeyboardState)
        {
            Keys[] keys = CurrentKeyboardState.GetPressedKeys();
            if (PreviousKeys == null)
                PreviousKeys = keys;
            bool IsUpper = (keys.Contains(Keys.LeftShift) || keys.Contains(Keys.RightShift));
            foreach (Keys currentKey in keys)
            {
                if (currentKey != Keys.None)
                {
                    if (PreviousKeys.Contains(currentKey))
                    {
                        if ((gameTime.TotalGameTime.TotalMilliseconds - timer > 125)) //125
                            HandleKey(gameTime, currentKey, IsUpper);
                    }
                    else if (!PreviousKeys.Contains(currentKey))
                    {
                        HandleKey(gameTime, currentKey, IsUpper);
                    }
                }
            }
            previousState = CurrentKeyboardState;
            PreviousKeys = keys;
        }

        private void HandleKey(GameTime gameTime, Keys CurrentKey, bool IsUpperCase)
        {
            string keyString = CurrentKey.ToString();
            if (CurrentKey == Keys.Space)
                InternalText += " ";
            else if (CurrentKey == Keys.Back && InternalText.Length > 0)
                InternalText = InternalText.Remove(InternalText.Length - 1);
            else if (CurrentKey == Keys.Enter)
                _focused = false;
            else if (keyString.Length == 1)
                InternalText += ToogleCase(IsUpperCase, keyString);
            else
                InternalText += HandleNumbers(keyString, IsUpperCase);

            timer = gameTime.TotalGameTime.TotalMilliseconds;

            if (_focused)
                TriggerTextChangedEvent();
        }

        private string HandleSpecialChars(string CurrentKey)
        {
            return string.Empty;
        }

        private string HandleNumbers(string CurrentKey, bool IsUpperCase)
        {
            if (CurrentKey.ToUpper().StartsWith("NUMPAD"))
            {
                return CurrentKey.Last().ToString();
            }
            else if (CurrentKey.ToUpper().StartsWith("D"))
                if (IsUpperCase)
                    return HandleSpecialChars(CurrentKey);
                else
                    return CurrentKey.Last().ToString();

            return string.Empty;
        }

        private void GetInput(KeyboardState CurrentKeyboardState)
        {
            Keys[] CurrentKeys = CurrentKeyboardState.GetPressedKeys();
            if (PreviousKeys != null)
                foreach (Keys Key in PreviousKeys)
                {
                    if (CurrentShiftTick <= 0)
                    {
                        UpperCase = (CurrentKeys.Contains(Keys.LeftShift) || CurrentKeys.Contains(Keys.RightShift));
                        if (UpperCase)
                            CurrentShiftTick = DelTick * 3;
                    }

                    if (Key == Keys.Back)
                    {
                        if (CurrentDelTick <= 0)
                        {
                            CurrentDelTick = DelTick;
                            if (InternalText.Length > 0)
                            {
                                InternalText = InternalText.Remove(InternalText.Length - 1, 1);
                                TriggerTextChangedEvent();
                            }
                        }
                        CurrentDelTick--;
                    }
                    HandleToggleKey(CurrentKeys, Key);
                    if (PreviousKeys.Contains(Key))
                    {
                        string KeyString = Key.ToString();
                        if (KeyString.Length == 1)
                        {
                            InternalText += ToogleCase(UpperCase, KeyString);
                            TriggerTextChangedEvent();
                        }
                    }
                    //if (CurrentKeyTick > 0)
                    //    CurrentKeyTick--;
                    //if (CurrentKeyTick <= 0)
                    //{
                    //    if (FirstClick)
                    //    {
                    //        CurrentKeyTick = DelTick * 3;
                    //        FirstClick = false;
                    //    }
                    //    else
                    //        CurrentKeyTick = DelTick;
                    //
                    //    if (Key.ToString().Length == 1)
                    //    {
                    //        InternalText += ToogleCase(UpperCase, Key.ToString());
                    //        TriggerTextChangedEvent();
                    //    }
                    //}

                    //
                    //    
                    //if (CurrentKeyTick <= 0 || !CurrentKeys.Contains(Key))
                    //{
                    //    CurrentKeyTick = DelTick * 2;
                    //    if (Key == Keys.Enter)
                    //    {
                    //        _focused = false;
                    //    }
                    //    else if (Key == Keys.Space)
                    //    {
                    //        InternalText += " ";
                    //        TriggerTextChangedEvent();
                    //    }
                    //
                    //
                    //}
                }
            PreviousKeys = CurrentKeys;
            if (CurrentShiftTick > 0)
                CurrentShiftTick--;

        }

        private void Test()
        {
            Keys test = Keys.None;
            switch (test)
            {
                case Keys.CapsLock:
                    break;

                case Keys.Delete:
                    break;

                case Keys.Multiply:
                    break;
                case Keys.Add:
                    break;
                case Keys.Separator:
                    break;
                case Keys.Subtract:
                    break;
                case Keys.Decimal:
                    break;
                case Keys.Divide:
                    break;

                case Keys.LeftShift:
                    break;
                case Keys.RightShift:
                    break;


                case Keys.LeftControl:
                    break;
                case Keys.RightControl:
                    break;


                case Keys.OemSemicolon:
                    break;
                case Keys.OemPlus:
                    break;
                case Keys.OemComma:
                    break;
                case Keys.OemMinus:
                    break;
                case Keys.OemPeriod:
                    break;
                case Keys.OemQuestion:
                    break;
                case Keys.OemTilde:
                    break;
                case Keys.OemOpenBrackets:
                    break;
                case Keys.OemPipe:
                    break;
                case Keys.OemCloseBrackets:
                    break;
                case Keys.OemQuotes:
                    break;
                case Keys.Oem8:
                    break;
                case Keys.OemBackslash:
                    break;
                case Keys.ProcessKey:
                    break;
                case Keys.Attn:
                    break;
                case Keys.Crsel:
                    break;
                case Keys.Exsel:
                    break;
                case Keys.EraseEof:
                    break;
                default:
                    break;
            }
        }

        private bool HandleToggleKey(Keys[] CurrentKeys, Keys CurrentKey)
        {
            if (!CurrentKeys.Contains(CurrentKey))
            {
                if (CurrentKey == Keys.Enter)
                {
                    _focused = false;
                }
                return true;
            }
            return false;
        }

        private void TriggerTextChangedEvent()
        {
            EventHandler hander = TextChanged;
            if (hander != null)
            {
                hander(this, new EventArgs());
            }
        }

        private string ToogleCase(bool IsUpperCase, string key)
        {
            if (IsUpperCase)
                return key.ToUpper();
            else
                return key.ToLower();

        }

        private void GenerateShownText()
        {
            Vector2 StringSize = Settings.GlobalFont.MeasureString(InternalText);
            if (StringSize.X + LeftXOffset > Element.Width)
            {
                ShownText = CutString();
                StringSize = Settings.GlobalFont.MeasureString(ShownText);
                CurrentXOffset = Element.Width - ((int)StringSize.X + LeftXOffset);
            }
            else
                ShownText = InternalText;
        }

        private string CutString()
        {
            bool IsToLarge;
            string CurrentString = InternalText;
            do
            {
                Vector2 CurrentStringSize = Settings.GlobalFont.MeasureString(CurrentString);
                if (CurrentStringSize.X + LeftXOffset > Element.Width)
                {
                    IsToLarge = true;
                    if (CurrentString.Length > 0)
                        CurrentString = CurrentString.Remove(0, 1);
                    else
                        IsToLarge = false;
                }
                else
                    IsToLarge = false;
            } while (IsToLarge);
            return CurrentString;
        }

        public override void Draw(SpriteBatch SB)
        {
            if (_hidden)
                return;
            base.Draw(SB);
            Vector2 FontSize = Settings.GlobalFont.MeasureString(ShownText);
            SB.DrawString(Settings.GlobalFont, ShownText, GetDrawPosition(FontSize), TextColor);
            if (t2dBlinkingMarker != null && DrawBlink)
            {
                SB.Draw(t2dBlinkingMarker, new Rectangle((int)(Element.X + LeftXOffset + FontSize.X), Element.Y + Element.Height / 2 - BlinkingCursorHeight / 2, BlinkingCursorWidth, BlinkingCursorHeight), Color.White);
            }
        }

        private Vector2 GetDrawPosition(Vector2 FontSize)
        {
            Vector2 DrawPosition = new Vector2();
            float LeftStartPosition = (Element.X + LeftXOffset) + CurrentXOffset;
            switch (DrawTextPosition)
            {
                case TextPosition.Left:
                    DrawPosition = new Vector2(LeftStartPosition, Element.Y + Element.Height / 2 - FontSize.Y / 2);
                    break;
                case TextPosition.Center:
                    DrawPosition = new Vector2(LeftStartPosition + (Element.Width / 2 - FontSize.X / 2), Element.Y + Element.Height / 2 - FontSize.Y / 2);
                    break;
                case TextPosition.Right:
                    DrawPosition = new Vector2(LeftStartPosition + Element.Width - FontSize.X, Element.Y + Element.Height / 2 - FontSize.Y / 2);
                    break;
                default:
                    break;
            }
            return DrawPosition;
        }
    }
}

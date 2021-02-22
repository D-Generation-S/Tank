using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tank.Code.GUIClasses
{
    public class GUITextarea : GUIPrimitiv
    {
        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = getShownText(value);
            }
        }
        public Color TextColor
        {
            get;
            set;
        }
        public int[] Padding;

        public GUITextarea(int x, int y, float width, float height, int[] padding)
            : base(x, y, (int)width, (int)height)
        {
            Padding = padding;
        }

        public override void Draw(SpriteBatch SB)
        {
            base.Draw(SB);


            if (!string.IsNullOrEmpty(Text) && TextColor != null)
                SB.DrawString(Settings.GlobalFont, Text, new Vector2(Position.X + Padding[3], Position.Y + Padding[0]), TextColor);
        }

        private string getShownText(string text)
        {
            string strReturn = string.Empty;
            int lineIndex = 0;
            bool shortText = Settings.GlobalFont.MeasureString(text).X < Element.Width - Padding[1] - Padding[3];

            if (shortText)
                return text;

            for (int i = 0; i < text.Length; i++)
            {
                string testText = text.Substring(lineIndex, i - lineIndex);
                Vector2 StringSize = Settings.GlobalFont.MeasureString(testText);

                if (StringSize.X > Element.Width - Padding[1] - Padding[3])
                {
                    if (testText.Contains(' '))
                    {
                        for (int b = testText.Length - 1; b > 0; b--)
                        {
                            if (testText[b] == ' ')
                            {
                                strReturn += testText.Remove(b) + Environment.NewLine;
                                lineIndex = i - (testText.Length - b) + 1;
                                break;
                            }
                        }
                    }
                    else
                    {
                        strReturn += testText.Remove(testText.Length - 1) + Environment.NewLine;
                        lineIndex = i - 1;
                    }
                }
                if (text.Length - 1 == i)
                {
                    shortText = StringSize.X < Element.Width - Padding[1] - Padding[3];
                    if (shortText)
                    {
                        strReturn += testText;
                        break;
                    }
                }

            }

            return strReturn;
        }
    }
}

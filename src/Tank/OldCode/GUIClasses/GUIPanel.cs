using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tank.Code.GUIClasses
{
    public class GUIPanel : GUIPrimitiv
    {
        private Texture2D _panelContent;
        public Texture2D PanelContent
        {
            set
            {
                _panelContent = value;
            }
        }

        public GUIPanel(int PositionX, int PositionY, int Width, int Height, Texture2D Frame = null, bool Overlayer = false) : base(PositionX, PositionY, Width, Height, Frame, Overlayer)
        {

        }

        public override void Draw(SpriteBatch SB)
        {


            base.Draw(SB);
            if (_panelContent != null)
            {
                Vector2 ContentPosition = new Vector2((Position.X + Size.X / 2) - _panelContent.Width / 2, (Position.Y + Size.Y / 2) - _panelContent.Height / 2);
                SB.Draw(_panelContent, ContentPosition, Color.White);
            }
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState, GameTime currentGameTime, GamePadState gsState)
        {
            base.Update(mouseState, keyboardState, currentGameTime, gsState);
        }

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }
    }
}

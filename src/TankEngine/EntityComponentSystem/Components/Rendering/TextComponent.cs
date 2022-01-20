using Microsoft.Xna.Framework.Graphics;

namespace TankEngine.EntityComponentSystem.Components.Rendering
{
    public class TextComponent : AbstractRenderingComponent
    {
        public string Text { get; set; }

        public SpriteFont Font { get; set; }

        public float Scale { get; set; }

        public override void Init()
        {
            base.Init();
            Text = string.Empty;
            Font = null;
            Scale = 1;
        }
    }
}

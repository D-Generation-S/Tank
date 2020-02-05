using Microsoft.Xna.Framework;

namespace Tank.Interfaces.Components
{
    interface IVisible : IDrawable
    {
        IRenderObj Renderer { get; }
    }
}

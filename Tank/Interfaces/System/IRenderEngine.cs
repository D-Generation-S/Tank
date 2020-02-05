using Microsoft.Xna.Framework;
using Tank.Interfaces.Components;
using Tank.Interfaces.Implementations;

namespace Tank.Interfaces.System
{
    interface IRenderEngine : IDrawable
    {
        void AddRenderer(IVisible visibleEntity);

        void AddRenderer(IRenderer renderer);
    }
}

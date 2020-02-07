using Microsoft.Xna.Framework;
using Tank.Interfaces.Implementations;

namespace Tank.Interfaces.Components
{
    interface IVisible
    {
        float Rotation { get; set; }
        Vector2 RotationAxis { get; set; }

        IRenderer Renderer { get; }
    }
}

using Tank.Interfaces.Implementations;

namespace Tank.Interfaces.Components
{
    interface IVisible
    {
        IRenderer Renderer { get; }
    }
}

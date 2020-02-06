using Microsoft.Xna.Framework;

namespace Tank.Interfaces.Components
{
    interface IMoveable : IPlaceable
    {
        Vector2 Velocity { get; set; }
    }
}

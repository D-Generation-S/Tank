using Microsoft.Xna.Framework;

namespace Tank.Interfaces.Entity
{
    interface IEntity : IUpdateable
    {
        Vector2 Position { get; }

        void Reset();
    }
}
